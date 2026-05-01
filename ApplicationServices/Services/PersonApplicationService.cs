using PhoneBookApplication.ApplicationServices.DTOs;
using PhoneBookApplication.ApplicationServices.Services.Contracts;
using PhoneBookApplication.Models.DomainModels.PersonAggregates;
using PhoneBookApplication.Models.DomainModels;
using PhoneBookApplication.Models.Services.Contracts;
using ResponseFramework;
using System;
using System.Net;
using System.Globalization;



namespace PhoneBookApplication.ApplicationServices.Services
{
    public class PersonApplicationService : IPersonApplicationService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PersonApplicationService(IPersonRepository personRepository, IImageRepository imageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _personRepository = personRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageRepository = imageRepository;
        }


        #region [- Post() -]
        public async Task<IResponse<bool>> Post(PostPersonDto postPersonDto)
        {

            if (postPersonDto == null)
                return new Response<bool>("Request body cannot be null.");
            if (string.IsNullOrWhiteSpace(postPersonDto.FirstName))
                return new Response<bool>("First Name is a required field.");
            if (string.IsNullOrWhiteSpace(postPersonDto.LastName))
                return new Response<bool>("Last Name is a required field.");
            if (string.IsNullOrWhiteSpace(postPersonDto.PhoneNumber))
                return new Response<bool>("Phone Number is a required field.");
            if (postPersonDto.PhoneNumber.Length != 11)
                return new Response<bool>("Phone Number must be 11 digits.");


            var stringBirthDate = postPersonDto.BirthDate.ToString();

            stringBirthDate = stringBirthDate.Split(' ')[0];
            var splitBirthDate = stringBirthDate.Split('-', '/');

            int year = int.Parse(splitBirthDate[0]);
            int month = int.Parse(splitBirthDate[1]);
            int day = int.Parse(splitBirthDate[2]);

            PersianCalendar pc = new PersianCalendar();

            var gregorianBirthDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);


            if (postPersonDto.UploadFile != null)
            {
                string uniqueFileName = Guid.NewGuid() + "-" + postPersonDto.UploadFile.FileName;

                string targetPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                using (var stream = new FileStream(targetPath, FileMode.Create))
                    await postPersonDto.UploadFile.CopyToAsync(stream);


                postPersonDto.FilePath = uniqueFileName;


                byte[] imageContent;
                MemoryStream memoryStream = new MemoryStream();
                await postPersonDto.UploadFile.CopyToAsync(memoryStream);

                memoryStream.Position = 0;
                imageContent = memoryStream.ToArray();

                var image = new Image
                {
                    ImageBinaryData = imageContent,
                };

                await _imageRepository.Insert(image);

            }

            var person = new Person
            {
                FirstName = postPersonDto.FirstName,
                LastName = postPersonDto.LastName,
                PhoneNumber = postPersonDto.PhoneNumber,
                BirthDate = gregorianBirthDate,
                FilePath = postPersonDto.FilePath,




            };


            await _personRepository.Insert(person);

            return new Response<bool>(true);
        }
        #endregion

        #region [- GetById() -]
        public async Task<IResponse<GetByIdPersonDto>> GetByIdPerson(GetByIdPersonDto getByIdPersonDto)
        {

            if (getByIdPersonDto == null)
                return new Response<GetByIdPersonDto>("Request body cannot be null.");
            if (getByIdPersonDto.Id == Guid.Empty)
                return new Response<GetByIdPersonDto>("Person ID is required.");


            var personResponse = await _personRepository.SelectById(getByIdPersonDto.Id);

            if (!personResponse.IsSuccessful || personResponse.Result == null)
                return new Response<GetByIdPersonDto>("Person not found.");



            var person = personResponse.Result;

            var result = new GetByIdPersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.PhoneNumber,
                BirthDate = person.BirthDate,
                FilePath = person.FilePath,

            };

            return new Response<GetByIdPersonDto>(result);
        }
        #endregion

        #region [- GetAll() -]
        public async Task<IResponse<GetAllPersonDto>> GetAllPerson()
        {

            var response = await _personRepository.SelectAll();

            if (!response.IsSuccessful || response.Result == null)
                return new Response<GetAllPersonDto>(response.ErrorMessage ?? "Failed to retrieve persons.");

            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();

            var persons = response.Result.Select(person => new GetByIdPersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.PhoneNumber,
                BirthDate = person.BirthDate,
                FilePath = person.FilePath,

            }).ToList();

            var result = new GetAllPersonDto
            {
                GetByIdPersonDto = persons
            };

            return new Response<GetAllPersonDto>(result);
        }
        #endregion

        #region [- Search() -]
        public async Task<IResponse<List<GetByIdPersonDto>>> SearchPerson(string term)
        {
            await GetAllPerson();


            var repositoryResponse = await _personRepository.Search(term);

            if (!repositoryResponse.IsSuccessful)
            {
                return new Response<List<GetByIdPersonDto>>(null, false, "Error occurred.", null, HttpStatusCode.InternalServerError);
            }

            var result = repositoryResponse.Result.Select(person => new GetByIdPersonDto()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.PhoneNumber
            }).ToList();



            return new Response<List<GetByIdPersonDto>>(result);
        }

        #endregion

        #region [- Put() -]
        public async Task<IResponse<bool>> Put(PutPersonDto putPersonDto)
        {

            if (putPersonDto == null)
                return new Response<bool>("Request body cannot be null.");
            if (putPersonDto.Id == Guid.Empty)
                return new Response<bool>("Person ID is required for an update.");
            if (string.IsNullOrWhiteSpace(putPersonDto.FirstName))
                return new Response<bool>("First Name is a required field.");
            if (string.IsNullOrWhiteSpace(putPersonDto.LastName))
                return new Response<bool>("Last Name is a required field.");
            if (string.IsNullOrWhiteSpace(putPersonDto.PhoneNumber))
                return new Response<bool>("Phone Number is a required field.");
            if (putPersonDto.PhoneNumber.Length != 11)
                return new Response<bool>("Phone Number must be 11 digits.");



            var personResponse = await _personRepository.SelectById(putPersonDto.Id);

            if (!personResponse.IsSuccessful || personResponse.Result == null)
                return new Response<bool>("Person not found to update.");



            var person = personResponse.Result;
            person.FirstName = putPersonDto.FirstName;
            person.LastName = putPersonDto.LastName;
            person.PhoneNumber = putPersonDto.PhoneNumber;
            person.BirthDate = putPersonDto.BirthDate;
            person.UploadFile = putPersonDto.UploadFile;
            person.FilePath = putPersonDto.FilePath;



            if (putPersonDto.UploadFile != null)
            {

                if (!string.IsNullOrEmpty(person.FilePath))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", person.FilePath);
                    if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
                }

                string uniqueFileName = Guid.NewGuid() + "-" + putPersonDto.UploadFile.FileName;
                string targetPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                using var stream = new FileStream(targetPath, FileMode.Create);
                await putPersonDto.UploadFile.CopyToAsync(stream);

                person.FilePath = uniqueFileName;


                byte[] imageContent;
                MemoryStream memoryStream = new MemoryStream();
                await putPersonDto.UploadFile.CopyToAsync(memoryStream);

                memoryStream.Position = 0;
                imageContent = memoryStream.ToArray();

                var image = new Image
                {
                    ImageBinaryData = imageContent,
                };

                await _imageRepository.Insert(image);
            }

            await _personRepository.Update(person);
            return new Response<bool>(true);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<bool>> Delete(DeletePersonDto deletePersonDto)
        {

            if (deletePersonDto == null)
                return new Response<bool>("Request body cannot be null.");
            if (deletePersonDto.Id == Guid.Empty)
                return new Response<bool>("Person ID is required for deletion.");



            var personResponse = await _personRepository.SelectById(deletePersonDto.Id);

            if (!personResponse.IsSuccessful || personResponse.Result == null)
                return new Response<bool>("Person not found to delete.");

            var person = personResponse.Result;

            if (!string.IsNullOrEmpty(person.FilePath))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", person.FilePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await _personRepository.Delete(personResponse.Result);
            return new Response<bool>(true);
        }
        #endregion
    }
}
