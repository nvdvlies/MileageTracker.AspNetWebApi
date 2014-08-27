using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MileageTracker.Models {
    public class Car {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumberPlate { get; set; }

        [Required]
        public string Make { get; set; }

        public string Model { get; set; }

        public string Remarks { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
