using System.ComponentModel.DataAnnotations;

namespace Pojisteni.Models
{
    public class Insurance
    {
        public int InsuranceId { get; set; }

        public int InsuranceNameId { get; set; }
        [Display(Name ="Pojištění")]
        public InsuranceName? InsuranceName { get; set; }
        [Display(Name ="Částka")]
        [Range(1, 1000000000, ErrorMessage = "{0} musí být mezi {1} a {2}")]
        [DisplayFormat(DataFormatString = "{0:#,##0 Kč}")]
        public int Castka { get; set; }
        [Display(Name ="Předmět pojištění")]
        public string predmetPojisteni { get; set; } = string.Empty;
        [Display(Name ="Platnost od")]
        public DateTime platnostOd { get; set; } = DateTime.MinValue;
        [Display(Name ="Platnost do")]
        public DateTime platnostDo { get; set; } = DateTime.MaxValue;

        public int PersonId { get; set; }
        [Display(Name = "Osoba")]
        public Person? Person { get; set; }
    }
}
