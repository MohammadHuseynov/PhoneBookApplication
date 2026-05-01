using PhoneBookApplication.ApplicationServices.DTOs;
using ResponseFramework;

namespace PhoneBookApplication.ApplicationServices.Services.Contracts
{
    public interface IPersonApplicationService
    {
        #region [- POST -]
        Task<IResponse<bool>> Post(PostPersonDto postPersonDto);
        #endregion

        #region [- GET -]
        Task<IResponse<GetByIdPersonDto>> GetByIdPerson(GetByIdPersonDto getByIdPersonDto);

        Task<IResponse<GetAllPersonDto>> GetAllPerson();
        #endregion

        #region [- SEARCH -]
        Task<IResponse<List<GetByIdPersonDto>>> SearchPerson(string term);
        #endregion

        #region [- PUT -]
        Task<IResponse<bool>> Put(PutPersonDto putPersonDto);
        #endregion

        #region [- DELETE -]
        Task<IResponse<bool>> Delete(DeletePersonDto deletePersonDto);
        #endregion
    }
}
