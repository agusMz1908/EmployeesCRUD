using EmployeeCRUD.Data;
using EmployeeCRUD.Models;
using EmployeeCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;


namespace EmployeeCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _applicationDbContext.Employees.ToListAsync();
            return View(employees);
            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest) 
        {
            if (ModelState.IsValid) 
            {
                if (await _applicationDbContext.Employees.AnyAsync(e => e.Email == addEmployeeRequest.Email)) 
                {
                    ModelState.AddModelError("Email", "This employee alredy exist");
                    return View(addEmployeeRequest);
                }

                if (ModelState.IsValid) 
                { 
                    var employee = new Employee()
                    {
                        Id = Guid.NewGuid(),
                        Name = addEmployeeRequest.Name,
                        Email = addEmployeeRequest.Email,
                        Salary = addEmployeeRequest.Salary,
                        DateOfBirth = addEmployeeRequest.DateOfBirth,
                        Department = addEmployeeRequest.Department
                    };

                    await _applicationDbContext.Employees.AddAsync(employee);
                    await _applicationDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }


                }
            return View(addEmployeeRequest);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel(_applicationDbContext);

                viewModel.Id = employee.Id;
                viewModel.Name = employee.Name;
                viewModel.Email = employee.Email;
                viewModel.Salary = employee.Salary;
                viewModel.DateOfBirth = employee.DateOfBirth;
                viewModel.Department = employee.Department;

                if (!await viewModel.IsEmailUnique(viewModel.Id, viewModel.Email))
                {
                    ModelState.AddModelError("Email", "This email already exists.");
                }

                return View("View", viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _applicationDbContext.Employees.FindAsync(model.Id);

                if (employee != null)
                {
                    if (await IsEmailUnique(model.Id, model.Email))
                    {
                        employee.Name = model.Name;
                        employee.Email = model.Email;
                        employee.Salary = model.Salary;
                        employee.DateOfBirth = model.DateOfBirth;
                        employee.Department = model.Department;

                        await _applicationDbContext.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "This email already exists.");
                    }
                }
            }

            return await Task.Run(() => View("View", model));
        }

        private async Task<bool> IsEmailUnique(Guid id, string email)
        {
            var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Id != id);
            return employee == null;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await _applicationDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                _applicationDbContext.Employees.Remove(employee);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}


