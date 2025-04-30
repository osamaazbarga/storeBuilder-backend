namespace superecommere.Models.Store
{
    public class CreateStoreRequest:BaseEntity
    {
        public string Name { get; set; }
        public string Subdomain { get; set; }
    }
}
