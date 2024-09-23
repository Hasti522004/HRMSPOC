namespace HRMSPOC.WEB.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
