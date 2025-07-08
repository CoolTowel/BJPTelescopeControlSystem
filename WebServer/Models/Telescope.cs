using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Telescope
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // 可用 JSON 或 "纬度,经度" 表示位置
        public double Longitude { get; set; }

        public  double Latitude { get; set; }

        public bool IsOnline { get; set; } = false;

        public DateTime? LastHeartbeat { get; set; }

        public string? Description { get; set; }
    }
}
