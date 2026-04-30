namespace PhoneBookApplication.Models.DomainModels
{
    public class Image
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public BinaryData ImageBinaryData { get; set; }
    }
}
