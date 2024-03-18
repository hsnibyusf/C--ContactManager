using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace training3.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
       List<Address> Addresses { get; set;} = new List<Address>();
        List<Contact> Contacts { get; set;}= new List<Contact>();
        List<Phone> Phones { get; set; } = new List<Phone>();

        
    }
}
