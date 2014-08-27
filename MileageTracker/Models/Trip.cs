using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MileageTracker.Models {
    public class Trip {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public virtual Address AddressOrigin { get; set; }

        [Required]
        public virtual Address AddressDestination { get; set; }

        [Required]
        public virtual Car Car { get; set; }

        public string Remarks { get; set; }

        public int DistanceInKm { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
