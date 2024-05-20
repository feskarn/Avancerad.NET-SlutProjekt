using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Avancerad.NET_SlutProjekt.DTO
{
    public class AppointmentDto
    {
        [Key]
        public int AppointId { get; set; }
        public DateTime StartingTime { get; set; }
        public int AppointmentDurationMinutes { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        [JsonIgnore]
        public CustomerDto Customer { get; set; }

        public int CompanyId { get; set; }
        [JsonIgnore]
        public CompanyDto Company { get; set; }
    }
}
