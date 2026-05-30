using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refresh.Core.Entities;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    
    public bool IsDeleted { get; set; }
}