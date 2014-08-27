using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MileageTracker.Models {
    public class Address {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        public string Remarks { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
