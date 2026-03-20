using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Namespace.SoccerPro.Application.Common.Behaviours;
using Namespace.SoccerPro.Application.Services.PlayerServices;
using SoccerPro.Application.Services;
using SoccerPro.Application.Services.IServises;
using System.Reflection;
using System.Text;

namespace SoccerPro.Infrastructure.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            // 1) Bind JwtSettings (now including SecretKey)
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));


            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettingsSection["SecretKey"];
            var keyBytes = Encoding.UTF8.GetBytes(secretKey!);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    //You can set this to true for production.
                    //This setting determines whether to validate the "issuer" (iss) claim of the JWT. The "issuer" is the entity that issued the JWT. In this code, it's set to true, which means the issuer will be validated. In a production environment, you should typically set this to true to ensure that the token is issued by a trusted authority.
                    ValidateAudience = true,
                    //The "audience" (aud) claim in a JWT (JSON Web Token) represents the intended recipient of the token.
                    //this setting determines whether to validate the "audience" (aud) claim of the JWT. The "audience" represents the intended recipient of the JWT. Again, it's set to true, but it's advisable to set it to true in production to verify that the token is meant for your application.

                    ValidateLifetime = true,
                    // This setting ensures that the token has not expired. It's set to true so that the token's expiration time is checked during validation.

                    ValidateIssuerSigningKey = true,
                    //This setting determines whether to validate the signing key used to sign the JWT. Setting it to true ensures that the token's signature is verified with the specified key.

                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    //As we specified to validate Issuer and Audience, we must also specify the details of Audience and Issuer to validate the incoming token's issuer and audience against these details.
                    ValidAudience = jwtSettingsSection["Audience"],
                    ValidIssuer = jwtSettingsSection["Issuer"],
                    ClockSkew = TimeSpan.Zero

                };
            });



            //-----------------------------------------

            // Add MediaR in DI contianer
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // Add Fluent Validation in DI contianer
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Add Auto Mapper in DI contianer
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            //------------------------------------------
            // 4) Your application services
            services.AddScoped<IPlayerServices, PlayerServices>();
            services.AddScoped<ISharedServices, SharedServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<ITournamentServices, TournamentServices>();
            services.AddScoped<ITeamServices, TeamServices>();
            services.AddScoped<IManagerServices, ManagerServices>();
            services.AddScoped<IRequestServices, RequestServices>();
            services.AddScoped<IFieldServices, FieldServices>();
            services.AddScoped<ICoachServices, CoachServices>();            services.AddScoped<IMatchServices, MatchServices>();
            services.AddScoped<IRefereeServices, RefereeServices>();

            return services;
        }
    }
}
