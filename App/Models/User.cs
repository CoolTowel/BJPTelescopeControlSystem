using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class ApplicationUser: IdentityUser
    {
        // 添加你自己的字段，比如：
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? DisplayName { get; set; }
    }
    //public class User
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    public string Username { get; set; } = string.Empty;

    //    [Required]
    //    public string PasswordHash { get; set; } = string.Empty;

    //    [Required] 
    //    public string Salt { get; set; } = string.Empty;

    //    [Required]
    //    public string Role { get; set; } = "User"; // 或 "Admin"
    //    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //    public DateTime? LastLogin { get; set; } = null;

    //    public string? Email { get; set; } = null; // 可选字段
    //    public string? EmailConfirmation { get; set; } = null; // 可选字段
    //    public string? PhoneNumber { get; set; } = null; // 可选字段
    //    public string? PhoneNumberConfirmation { get; set; } = null; // 可选字段


    //}

}
