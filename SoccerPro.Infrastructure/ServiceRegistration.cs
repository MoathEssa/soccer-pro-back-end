using Microsoft.Extensions.DependencyInjection;
using Namespace.SoccerPro.Domain.IRepository;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.IRepository;
using SoccerPro.Infrastructure.Repository;

namespace SoccerPro.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services)
        {

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISharedRepository, SharedRepossitory>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IFieldRepository, FieldRepository>();
            services.AddScoped<ICoachRepository, CoachRepository>();            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IRefereeRepository, RefereeRepository>();

            return services;
        }
    }

}
