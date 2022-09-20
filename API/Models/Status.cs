namespace API.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book>? Book { get; set; }
    }
}