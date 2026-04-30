namespace PhoneBookApplication.ApplicationServices.DTOs
{
    public class SearchPersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Term { get; set; }
    }
}
