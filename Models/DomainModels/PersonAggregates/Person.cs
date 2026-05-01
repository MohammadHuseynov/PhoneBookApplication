using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace PhoneBookApplication.Models.DomainModels.PersonAggregates
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Phone]
        [MaxLength(11)]
        [RegularExpression(@"^\+?[\d\s\-\(\)]{11}$") ]
        public string PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; } 

        [NotMapped]
        public IFormFile? UploadFile { get; set; }
        public string? FilePath { get; set; }


    }
}
