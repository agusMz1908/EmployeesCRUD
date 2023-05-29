﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeCRUD.Models
{
    public class AddEmployeeViewModel
    {
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
    }
}
