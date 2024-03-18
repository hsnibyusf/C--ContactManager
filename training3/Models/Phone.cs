using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace training3.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        public string? Type { get; set; }

        public int ContactId { get; set; }
        [Required]
        public string ContactName { get; set; }=string.Empty;

        [ForeignKey(nameof(ContactId))]
        public Contact? Contact { get; set; }
    }
}
