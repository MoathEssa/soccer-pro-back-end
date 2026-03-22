// ──────────────────────────────────────────────────────────────────────────────
// SoccerPro – Azure Container Apps Infrastructure (Bicep)
// Free consumption tier – $0 for portfolio / demo workloads
// ──────────────────────────────────────────────────────────────────────────────

@description('Base name used to derive all resource names (e.g. soccerpro)')
param appName string

@description('Azure region for all resources')
param location string = 'eastus'

@description('Container image to deploy (e.g. ghcr.io/moathessa/soccer-pro-back-end:latest)')
param containerImage string

// ── Secrets / App Settings (injected from GitHub Secrets) ───────────────────

@secure()
@description('SQL Server connection string')
param connectionStringDefault string = ''

@secure()
@description('JWT signing key – at least 32 characters')
param jwtSecretKey string = ''

@description('JWT issuer')
param jwtIssuer string = 'https://api.SoccerPro.com'

@description('JWT audience')
param jwtAudience string = 'SoccerPro-client'

@description('Frontend base URL for CORS')
param frontendBaseUrl string = 'http://localhost:5173'

// ── Container Apps Environment (consumption – free tier) ────────────────────

resource environment 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: '${appName}-env'
  location: location
  properties: {}
}

// ── Container App ───────────────────────────────────────────────────────────

resource containerApp 'Microsoft.App/containerApps@2024-03-01' = {
  name: '${appName}-api'
  location: location
  properties: {
    managedEnvironmentId: environment.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'http'
      }
      secrets: [
        {
          name: 'connection-string'
          value: connectionStringDefault
        }
        {
          name: 'jwt-secret-key'
          value: jwtSecretKey
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
          image: containerImage
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            // ── Connection Strings ──
            {
              name: 'ConnectionStrings__DefaultConnection'
              secretRef: 'connection-string'
            }
            // ── JWT ──
            {
              name: 'JwtSettings__SecretKey'
              secretRef: 'jwt-secret-key'
            }
            {
              name: 'JwtSettings__Issuer'
              value: jwtIssuer
            }
            {
              name: 'JwtSettings__Audience'
              value: jwtAudience
            }
            {
              name: 'JwtSettings__ExpirationMinutes'
              value: '60'
            }
            {
              name: 'JwtSettings__RefreshTokenExpirationInDays'
              value: '7'
            }
            // ── App / CORS ──
            {
              name: 'AppSettings__FrontendBaseUrl'
              value: frontendBaseUrl
            }
            // ── ASP.NET Core environment ──
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 1
      }
    }
  }
}

// ── Outputs ─────────────────────────────────────────────────────────────────

output containerAppName string = containerApp.name
output containerAppUrl string = 'https://${containerApp.properties.configuration.ingress.fqdn}'
