using Microsoft.EntityFrameworkCore;
using PhoneBookApplication.Models.DomainModels;
using PhoneBookApplication.Models.Services.Contracts;
using ResponseFramework;
using System.Net;

namespace PhoneBookApplication.Models.Services.Repositories
{
    public class ImageRepository : IImageRepository
    {

        private readonly PhoneBookApplicationDbContext _context;

        public ImageRepository(PhoneBookApplicationDbContext context)
        {
            _context = context;
        }

        #region [- Insert(IMAGE) -]
        public async Task<IResponse<bool>> Insert(Image image)
        {
            await _context.AddAsync(image);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Inserting was successful", null, HttpStatusCode.Created);
        }
        #endregion

        #region [- SaveChanges() -]
        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region [- NotImplementedException -]

        public Task<IResponse<List<Image>>> Search(string term)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<Image>>> SelectAll()
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<Image>> SelectById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<bool>> Update(Image entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<bool>> Delete(Image entity)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
