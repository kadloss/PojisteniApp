using System.ComponentModel.DataAnnotations;

namespace Pojisteni.Models
{
    public class InsuranceName
    {
        public int InsuranceNameId { get; set; }
        [Display(Name ="Název pojištění")]
        public string nazevPojisteni { get; set; } = string.Empty;
    }
}
