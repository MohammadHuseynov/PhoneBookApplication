using ResponseFramework;

namespace PhoneBookApplication.Models.Services.Contracts.RepositoryFrameworks
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region [- INSERT -]
        Task<IResponse<bool>> Insert(TEntity entity);
        #endregion

        #region [- SELECT -]
        Task<IResponse<TEntity>> SelectById(Guid id);

        Task<IResponse<List<TEntity>>> SelectAll();
        #endregion

        #region [- SEARCH -]
        Task<IResponse<List<TEntity>>> Search(string term);
        #endregion

        #region [- UPDATE -]
        Task<IResponse<bool>> Update(TEntity entity);
        #endregion

        #region [- DELETE -]
        Task<IResponse<bool>> Delete(TEntity entity);
        #endregion

    }
}
