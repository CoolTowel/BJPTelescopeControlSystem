public record TelescopeDTO(
    string Name,
    double Longitude,
    double Latitude,
    string Description);

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TelescopesController : ControllerBase
{
    private readonly AppDbContext _db;

    public TelescopesController(AppDbContext db)
    {
        _db = db;
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTelescope(int id, [FromBody] TelescopeDTO req)
    {
        var telescope = await _db.Telescopes.FindAsync(id);
        if (telescope == null)
            return NotFound("望远镜不存在");

        // 更新字段
        telescope.Name = req.Name;
        telescope.Latitude = req.Latitude;
        telescope.Longitude = req.Longitude;
        telescope.Description = req.Description;

        await _db.SaveChangesAsync();
        return Ok("更新成功");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllTelescopes")]
    public async Task<IActionResult> GetAllTelescopes()
    {
        var telescopes = await _db.Telescopes.ToListAsync();
        return Ok(telescopes);
    }
}
