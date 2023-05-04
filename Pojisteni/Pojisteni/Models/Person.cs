using System.ComponentModel.DataAnnotations;

namespace Pojisteni.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        [Display(Name = "Jméno")]
        public string jmenoPojistence { get; set; } = string.Empty;
        [Display(Name = "Příjmení")]
        public string prijmeniPojistence { get; set; } = string.Empty;
        [Display(Name = "Jméno")]
        public string celeJmeno { get { return $"{jmenoPojistence} {prijmeniPojistence}"; } }

        public string Email { get; set; } = string.Empty;

        public string Telefon { get; set; } = string.Empty;
        [Display(Name = "Ulice a číslo popisné")]
        public string Ulice { get; set; } = string.Empty;
        [Display(Name = "Město")]
        public string Mesto { get; set; } = string.Empty;
        [Display(Name = "PSČ")]
        public string PSC { get; set; } = string.Empty;

        public string Adresa { get { return $"{Ulice} {Mesto}"; } }

        public string celeJmenosAdresou { get { return $"{celeJmeno} {Adresa}"; } }

        public List<Insurance> Insurances { get; set; } = new List<Insurance>();
    }
}
