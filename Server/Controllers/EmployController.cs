using BlazorAppTestWasm.Server.Models;
using BlazorAppTestWasm.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTestWasm.Server.Controllers
{
    [Route("api/[controller]")] // api/Employee/
    [ApiController]
    public class EmployController : ControllerBase
    {
        private PostgresContext _postgresContext = new PostgresContext();
        [HttpGet]
        [Route("read")]
        public IEnumerable<Employee> GetEmployees()
        { 
            return this._postgresContext.Employees.ToList();
        }
        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> Post([FromBody] Employee employee) // 클라이언트에 전달받은 데이터를 
        {
            if (employee == null) //서버에서 db로 insert
            {
                return BadRequest();
            }

            this._postgresContext.Add(employee); // db에 클라이언트로 전달받은 employee를 insert

            try
            {
                await this._postgresContext.SaveChangesAsync(); // db에 변경점 저장

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // 500: Internal Server Error
            }
        }
    }
}
