using superecommere.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace superecommere.Models.Store
{
    public class TblStore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Kind { get; set; }
        public string Category { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public TblUser User { get; set; }
        public string UserId { get; set; }// Foreign key
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;



    }
}
