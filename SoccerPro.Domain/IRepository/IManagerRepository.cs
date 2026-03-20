using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Domain.IRepository;

public interface IManagerRepository
{
    Task<int?> AddManagerAsync(Manager manager);
    Task<ManagerView?> GetManagerByIdAsync(int managerId);
    public Task<(List<ManagerSearchView> managers, int totalCount)> SearchManagersAsync(
            string? kfupmId = null,
            string? firstName = null,
            string? secondName = null,
            string? thirdName = null,
            string? lastName = null,
            DateTime? dateOfBirth = null,
            int? nationalityId = null,
            string? teamName = null,
            int pageNumber = 1,
            int pageSize = 10);
}