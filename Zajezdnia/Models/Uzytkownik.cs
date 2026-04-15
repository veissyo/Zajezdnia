using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Zajezdnia.Models;
[Index(nameof(Login), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Uzytkownik
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Login { get; set; } = "";

    [Required]
    public string HasloHash { get; set; } = "";

    [Required, StringLength(50)]
    public string Imie { get; set; } = "";

    [Required, StringLength(50)]
    public string Nazwisko { get; set; } = "";

    [Required, StringLength(100), EmailAddress]
    public string Email { get; set; } = "";

    public DateTime DataRejestracji { get; set; } = DateTime.UtcNow;
}