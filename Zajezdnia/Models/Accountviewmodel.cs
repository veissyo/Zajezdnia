using System.ComponentModel.DataAnnotations;

namespace Zajezdnia.Models;

public class RejestrujViewModel
{
    [Required(ErrorMessage = "Login jest wymagany")]
    [StringLength(50)]
    [Display(Name = "Login")]
    public string Login { get; set; } = "";

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Haslo { get; set; } = "";

    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [DataType(DataType.Password)]
    [Compare("Haslo", ErrorMessage = "Hasła nie są zgodne")]
    [Display(Name = "Potwierdź hasło")]
    public string PotwierdzHaslo { get; set; } = "";

    [Required(ErrorMessage = "Imię jest wymagane")]
    [StringLength(50)]
    [Display(Name = "Imię")]
    public string Imie { get; set; } = "";

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [StringLength(50)]
    [Display(Name = "Nazwisko")]
    public string Nazwisko { get; set; } = "";

    [Required(ErrorMessage = "Email jest wymagany")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres email")]
    [Display(Name = "Adres email")]
    public string Email { get; set; } = "";
}

public class ZalogujViewModel
{
    [Required(ErrorMessage = "Login jest wymagany")]
    [Display(Name = "Login")]
    public string Login { get; set; } = "";

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Haslo { get; set; } = "";

    [Display(Name = "Zapamiętaj mnie")]
    public bool ZapamiętajMnie { get; set; }
}