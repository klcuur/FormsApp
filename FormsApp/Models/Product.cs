using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
	public class Product
	{
		
		[Display(Name = "Urun Id")]
		public int ProductId { get; set; }

        [Display(Name = "Urun Adi")]
		[Required(ErrorMessage="Gerekli bir alan")]
        public string? Name { get; set; }

		[Required]
		[Display(Name = "Urun Fiyati")]
		[Range(0,10000000)]
		public decimal? Price { get; set; }

		
		[Display(Name = "Resim")]
		public string? Image { get; set; }
		[Display(Name = "Aktiflik")]
		public bool IsActive { get; set; }

		[Required]
		[Display(Name="Category")]
		public int? CategoryId { get; set; }
		
		//public IFormFile ImageFile { get; set; }


	}
}
