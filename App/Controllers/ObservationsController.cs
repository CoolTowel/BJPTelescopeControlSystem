using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using System.Text.Json.Serialization;

namespace App.Controllers
{
    // 观测任务DTO
    public class ObservationTaskDto
    {
        public required string TargetName { get; set; } 
        public required double RA { get; set; } // 格式: hh:mm:ss
        public required double Dec { get; set; } // 格式: dd:mm:ss
        public required int Telescope { get; set; }
        public required int LNums { get; set; }
        public required int LExpSecs { get; set; }
        public required int RNums { get; set; }
        public required int RExpSecs { get; set; }
        public required int GNums { get; set; }
        public required int GExpSecs { get; set; }
        public required int BNums { get; set; }
        public required int BExpSecs { get; set; }
        public required int HNums { get; set; }
        public required int HExpSecs { get; set; }
        public required int SNums { get; set; }
        public required int SExpSecs { get; set; }
        public required int ONums { get; set; }
        public required int OExpSecs { get; set; }
    }

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ObservationsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ObservationsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("CreateObservationTask")]
        public async Task<IActionResult> CreateObservationTask([FromBody] ObservationTaskDto req)
        {
            if (req == null)
            {
                return BadRequest("观测任务数据无效。");
            }
            // 检查所有必填字段是否为 null 或空字符串
            if (string.IsNullOrWhiteSpace(req.TargetName))
            {
                return BadRequest("目标名称不能为空。");
            }
            if (req.RA < 0 || req.RA > 24 || req.Dec < -90 || req.Dec > 90)
            {
                return BadRequest("赤经或赤纬不合理。" + req.RA + " " + req.Dec);
            }
            // 检查通道参数是否有为 null 或不合理的情况（这里只检查数量和曝光时间是否为负数或为0）
            if (req.LNums < 0 || req.LExpSecs < 0 ||
                req.RNums < 0 || req.RExpSecs < 0 ||
                req.GNums < 0 || req.GExpSecs < 0 ||
                req.BNums < 0 || req.BExpSecs < 0 ||
                req.HNums < 0 || req.HExpSecs < 0 ||
                req.SNums < 0 || req.SExpSecs < 0 ||
                req.ONums < 0 || req.OExpSecs < 0)
            {
                return BadRequest("通道数量和曝光时间不能为负数。");
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var observationtask = new ObservationTask
            {
                GUID = Guid.NewGuid(),
                ApplicantID = Guid.Parse(userId ?? "0"),
                ApplicantName = User.Identity?.Name ?? "Unknown",
                SubmitTime = DateTime.UtcNow,
                TargetName = req.TargetName,
                RA = req.RA,
                Dec = req.Dec,
                Telescope = req.Telescope,
                LNums = req.LNums,
                LExpSecs = req.LExpSecs,
                RNums = req.RNums,
                RExpSecs = req.RExpSecs,
                GNums = req.GNums,
                GExpSecs = req.GExpSecs,
                BNums = req.BNums,
                BExpSecs = req.BExpSecs,
                HNums = req.HNums,
                HExpSecs = req.HExpSecs,
                SNums = req.SNums,
                SExpSecs = req.SExpSecs,
                ONums = req.ONums,
                OExpSecs = req.OExpSecs
            };
            _db.ObservationTasks.Add(observationtask);  
            await _db.SaveChangesAsync();
            return Ok(new { message = "观测任务提交成功！" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllObservationTask")]
        public async Task<IActionResult> GetAllObservationTask()
        {
            var observationtasks = await _db.ObservationTasks.ToListAsync();
            return Ok(observationtasks);
        }
    }
}
