using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using System.Text.Json.Serialization;

namespace App.Controllers
{
    // 观测任务DTO
    public class ObservationTaskDto
    {
        [JsonPropertyName("TargetName")]
        public string TargetName { get; set; }
        [JsonPropertyName("RA")]
        public string RA { get; set; } // 格式: hh:mm:ss
        [JsonPropertyName("Dec")]
        public string Dec { get; set; } // 格式: dd:mm:ss
        [JsonPropertyName("Telescope")]
        public string Telescope { get; set; }
        [JsonPropertyName("L-Nums")]
        public int LNums { get; set; }
        [JsonPropertyName("L-ExpSecs")]
        public int LExpSecs { get; set; }
        [JsonPropertyName("R-Nums")]
        public int RNums { get; set; }
        [JsonPropertyName("R-ExpSecs")]
        public int RExpSecs { get; set; }
        [JsonPropertyName("G-Nums")]
        public int GNums { get; set; }
        [JsonPropertyName("G-ExpSecs")]
        public int GExpSecs { get; set; }
        [JsonPropertyName("B-Nums")]
        public int BNums { get; set; }
        [JsonPropertyName("B-ExpSecs")]
        public int BExpSecs { get; set; }
        [JsonPropertyName("H-Nums")]
        public int HNums { get; set; }
        [JsonPropertyName("H-ExpSecs")]
        public int HExpSecs { get; set; }
        [JsonPropertyName("S-Nums")]
        public int SNums { get; set; }
        [JsonPropertyName("S-ExpSecs")]
        public int SExpSecs { get; set; }
        [JsonPropertyName("O-Nums")]
        public int ONums { get; set; }
        [JsonPropertyName("O-ExpSecs")]
        public int OExpSecs { get; set; }
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

        [HttpPost]
        public async Task<IActionResult> CreateObservationTask([FromBody] ObservationTask observationtask)
        {
            if (observationtask == null)
            {
                return BadRequest("观测任务数据无效。");
            }
            // 检查所有必填字段是否为 null 或空字符串
            if (string.IsNullOrWhiteSpace(observationtask.TargetName) ||
                string.IsNullOrWhiteSpace(observationtask.RA) ||
                string.IsNullOrWhiteSpace(observationtask.Dec) ||
                string.IsNullOrWhiteSpace(observationtask.Telescope))
            {
                return BadRequest("目标名称、赤经或赤纬不能为空。");
            }

            // 检查通道参数是否有为 null 或不合理的情况（这里只检查数量和曝光时间是否为负数或为0）
            if (observationtask.LNums < 0 || observationtask.LExpSecs < 0 ||
                observationtask.RNums < 0 || observationtask.RExpSecs < 0 ||
                observationtask.GNums < 0 || observationtask.GExpSecs < 0 ||
                observationtask.BNums < 0 || observationtask.BExpSecs < 0 ||
                observationtask.HNums < 0 || observationtask.HExpSecs < 0 ||
                observationtask.SNums < 0 || observationtask.SExpSecs < 0 ||
                observationtask.ONums < 0 || observationtask.OExpSecs < 0)
            {
                return BadRequest("通道数量和曝光时间不能为负数。");
            }
            
            _db.ObservationTasks.Add(observationtask);  
            await _db.SaveChangesAsync();
            return Ok(new { message = "观测任务提交成功！" });
        }
    }
}
