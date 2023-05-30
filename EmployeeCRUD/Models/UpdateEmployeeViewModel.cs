using EmployeeCRUD.Models.Domain;
using EmployeeCRUD.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Models
{
    public class UpdateEmployeeViewModel
    {
        private ApplicationDbContext applicationDbContext;
        public UpdateEmployeeViewModel()
        {
            // Constructor sin parámetros necesario para el enlace de modelo
        }

        public UpdateEmployeeViewModel(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Guid Id { get; set; }
        [Required(ErrorMessage = "The Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Email is Required")]
        public string Email { get; set; }
        [DefaultValue(1)]
        [Range(25000, long.MaxValue, ErrorMessage = "Salary must be greater than 25.000")]
        public long Salary { get; set; }
        [Required(ErrorMessage = "The date is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "The department is Required")]
        public string Department { get; set; }

        public async Task<bool> IsEmailUnique(Guid id, string email)
        {
            var existingEmployee = await applicationDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Id != id);
            return existingEmployee == null;
        }
    }
}
