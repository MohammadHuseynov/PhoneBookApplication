using Microsoft.EntityFrameworkCore;
using PhoneBookApplication.Models.DomainModels.PersonAggregates;
using PhoneBookApplication.Models.Services.Contracts;
using ResponseFramework;
using System.Net;

namespace PhoneBookApplication.Models.Services.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PhoneBookApplicationDbContext _context;
       
        public PersonRepository(PhoneBookApplicationDbContext context)
        {
            _context = context;
        }

        #region [- Insert() -]
        public async Task<IResponse<bool>> Insert(Person person)
        {
            if (person == null)
                return new Response<bool>("Person cannot be null.") { HttpStatusCode = HttpStatusCode.BadRequest };

            await _context.AddAsync(person);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Inserting was successful", null, HttpStatusCode.Created);
        }
        #endregion

        #region [- SelectById() -]
        public async Task<IResponse<Person>> SelectById(Guid id)
        {
            if (id == Guid.Empty)
                return new Response<Person>("Id cannot be empty.") { HttpStatusCode = HttpStatusCode.BadRequest };

            var person = await _context.Person.FindAsync(id);

            if (person == null)
                return new Response<Person>(person, false, "Person not found", $"Person with ID '{id}' not found",
                    HttpStatusCode.NotFound);

            return new Response<Person>(person, true, "Person retrieved successfully", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- SelectAll() -]
        public async Task<IResponse<List<Person>>> SelectAll()
        {
            var persons = await _context.Person.AsNoTracking().ToListAsync();
            return new Response<List<Person>>(persons, true, "Persons retrieved successfully", null, HttpStatusCode.OK);
        }
        #endregion


        #region [- Search() -]
        public async Task<IResponse<List<Person>>> Search(string term)
        {
            var personsQuery = _context.Person.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                personsQuery = personsQuery.Where(x =>
                    (x.FirstName != null && x.FirstName.Contains(term)) ||
                    (x.LastName != null && x.LastName.Contains(term)) ||
                    (x.PhoneNumber != null && x.PhoneNumber.Contains(term))
                );
            }

            var persons = await personsQuery.ToListAsync();

            return new Response<List<Person>>(persons, true, "OK", null, HttpStatusCode.OK);
        }
        #endregion



        #region [- Update() -]
        public async Task<IResponse<bool>> Update(Person person)
        {
            if (person == null)
                return new Response<bool>("Person cannot be null.") { HttpStatusCode = HttpStatusCode.BadRequest };

            _context.Update(person);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Updating was successful", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<bool>> Delete(Person person)
        {
            if (person == null)
                return new Response<bool>("Person cannot be null.") { HttpStatusCode = HttpStatusCode.BadRequest };

            _context.Remove(person);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Deleting was successful", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- SaveChanges() -]
        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion


    }
}
