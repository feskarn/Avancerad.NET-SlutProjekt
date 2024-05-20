using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "This is not a valid email address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
