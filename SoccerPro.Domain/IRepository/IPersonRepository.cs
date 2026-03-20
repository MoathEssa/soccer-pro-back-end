using SoccerPro.Domain.Entities;

namespace SoccerPro.Infrastructure.Repository
{
    public interface IPersonRepository
    {
        public Task<int> AddPersonAsync(Person person);

        public Task<bool> CheckIsPersonExistAsync(string KFUPMId);
    }
}
