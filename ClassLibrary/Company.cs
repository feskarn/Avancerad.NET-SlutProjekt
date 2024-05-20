using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        [StringLength(25)]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(25)]
        public string CompanyEmail { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}
