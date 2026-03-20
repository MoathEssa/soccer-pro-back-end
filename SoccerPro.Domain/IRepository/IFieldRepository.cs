using SoccerPro.Domain.Entities;

namespace SoccerPro.Domain.IRepository;

public interface IFieldRepository
{
    Task<List<Field>> GetAllFieldsAsync();
}