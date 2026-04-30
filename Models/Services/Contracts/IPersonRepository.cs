using PhoneBookApplication.Models.DomainModels.PersonAggregates;
using PhoneBookApplication.Models.Services.Contracts.RepositoryFrameworks;

namespace PhoneBookApplication.Models.Services.Contracts
{
    public interface IPersonRepository : IRepository<Person>
    {

    }
}
