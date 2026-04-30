using System.Globalization;

namespace PhoneBookApplication.ApplicationServices.DTOs
{
    public class PutPersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public IFormFile? UploadFile { get; set; }
        public string? FilePath { get; set; }

    }
}
