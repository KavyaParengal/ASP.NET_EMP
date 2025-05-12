using APIExample.Context;
using APIExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APIExample.Controllers
{
    public class EmployeeDetailsController : Controller
    {
        private EmployeeModel _model;
        private DataContext _context;

        public EmployeeDetailsController(EmployeeModel model, DataContext context)
        {
            _model = model;
            _context = context;
        }

        [HttpGet("GetAllEmployeeDetails", Name = "GetAllEmployeeDetails")]
        public async Task<IActionResult> GetAllEmpDetails()
        {
            var cmd = "SELECT * FROM Employeenew";
            var employees = new List<EmployeeModel>();
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new EmployeeModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("Name")),
                            designation = reader.GetString(reader.GetOrdinal("designation")),
                            department = reader.GetString(reader.GetOrdinal("department"))
                        };
                        employees.Add(employee);
                    }
                }
            }
            return Ok(employees);
        }

        [HttpGet("GetAllEmployeeIDName", Name = "GetAllEmployeeIDName")]
        public async Task<IActionResult> GetAllEmployeeIDName()
        {
            var cmd = "SELECT id,name FROM Employeenew";
            var employees = new List<EmployeeModel>();
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new EmployeeModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("Name")),
                            
                        };
                        employees.Add(employee);
                    }
                }
            }
            return Ok(employees);
        }

        [HttpGet("SearchById/{id}", Name = "SearchById")]
        public async Task<IActionResult> SearchById([FromRoute] int id)
        {
            var cmd = "SELECT * FROM Employeenew WHERE id=@id";
            var employees = new List<EmployeeModel>();
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);
                //add the parameter to the command
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new EmployeeModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("Name")),
                            designation = reader.GetString(reader.GetOrdinal("designation")),
                            department = reader.GetString(reader.GetOrdinal("department"))

                        };
                        employees.Add(employee);
                    }
                }
            }
            return Ok(employees);
        }

        // Post employee
        [HttpPost("insertEmployee", Name = "insertEmployee")]
        public async Task<IActionResult> insertEmployee([FromBody] EmployeeModel employee)
        {
            if (employee == null || string.IsNullOrWhiteSpace(employee.name))
            {
                return BadRequest("Invalid employee data");
            }
            var cmd = "INSERT INTO Employeenew(Id,name,designation,department) VALUES (@id,@name,@designation,@department)";
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);

                //Add parameters to the command
                command.Parameters.AddWithValue("@id", employee.Id);
                command.Parameters.AddWithValue("@name", employee.name);
                command.Parameters.AddWithValue("@designation", employee.designation);
                command.Parameters.AddWithValue("@department", employee.department);

                //Execute the command
                int rowsAffected = await command.ExecuteNonQueryAsync();

                //check if the insert was successfull
                if (rowsAffected > 0)
                {
                    return Ok(" Insert succussfully and the EmpID is "+ employee.Id );
                }
                else
                {
                    return BadRequest("Failed to insert employee");
                }
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
