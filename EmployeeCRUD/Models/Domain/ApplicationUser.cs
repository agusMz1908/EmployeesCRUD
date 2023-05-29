using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeCRUD.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "The Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Address is Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The City is Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "The Country is Required")]
        public string Country { get; set; }
    }
}