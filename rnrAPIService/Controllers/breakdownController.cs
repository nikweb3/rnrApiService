using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace rnrAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class breakdownController : ControllerBase
    {
        private readonly IConfiguration _config;
        public breakdownController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Breakdown>>> GetAllBreakdowns()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Breakdown> breakdowns = await SelectAllBreakdowns(connection);
            return Ok(breakdowns);
        }

        private static async Task<IEnumerable<Breakdown>> SelectAllBreakdowns(SqlConnection connection)
        {
            return await connection.QueryAsync<Breakdown>("select * from breakdowns");
        }

        [HttpGet("{BreakdownRef}")]
        public async Task<ActionResult<Breakdown>> GetBreakdown(string BreakdownRef)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var breakdown = await connection.QueryFirstAsync<Breakdown>("select * from breakdowns where BreakdownRef = @Id", new {Id = BreakdownRef});
            return Ok(breakdown);
        }

        [HttpPost]
        public async Task<ActionResult<List<Breakdown>>> CreateBreakdown(Breakdown breakdown)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into breakdowns(BreakdownRef, CompanyName, DriverName, RegNum, BreakdownDate) values (@BreakdownRef, @CompanyName, @DriverName, @RegNum, @BreakdownDate)",breakdown);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<List<Breakdown>>> UpdateBreakdown(Breakdown breakdown)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update breakdowns set BreakdownRef = @BreakdownRef, CompanyName = @CompanyName, DriverName = @DriverName, RegNum = @RegNum, BreakdownDate = @BreakdownDate where BreakdownRef=@BreakdownRef", breakdown);
            return Ok();
        }

        [HttpDelete("{BreakdownRef}")]
        public async Task<ActionResult<List<Breakdown>>> UpdateBreakdown(string BreakdownRef)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from breakdowns where BreakdownRef=@BreakdownRef", new { BreakdownRef = BreakdownRef });
            return Ok();
        }

    }
}
