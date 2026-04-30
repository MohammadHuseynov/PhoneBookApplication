using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PhoneBookApplication.ApplicationServices.DTOs;

namespace PhoneBookApplication.ViewModels
{
    public class SearchPersonViewModel
    {

        public string? Term { get; set; }

        public SearchPersonDto SearchPersonVM { get; set; } = new SearchPersonDto();

        public GetAllPersonDto GetAllPersonVM { get; set; } = new GetAllPersonDto();
    }
}
