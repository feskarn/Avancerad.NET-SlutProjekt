using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ChangeLog
    {
        [Key]
        public int Id { get; set; }
        public int AppointId { get; set; }
        public DateTime ChangedAtDate { get; set; }

        public string Action { get; set; }
        public string Details { get; set; }

        public int? CustomerId { get; set; }

        public int? CompanyId { get; set; }


    }
}