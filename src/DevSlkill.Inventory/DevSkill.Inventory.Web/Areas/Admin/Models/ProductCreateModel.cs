using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductCreateModel
    {
		[Display(Name = "Name *"),Required, StringLength(100)]
		public string? Name { get; set; }

        [Display(Name = "Upload Image")]
        [Required(ErrorMessage = "Please upload an image.")]
        public IFormFile ImageFile { get; set; }

		[Required, StringLength(15)]
		public string? Barcode { get; set; }

		[Required, StringLength(100)]
		public string? Status { get; set; }

		[Display(Name = "Category"), Required]
		public Guid CategoryId { get; set; }

		[Display(Name = "Unit"), Required]
		public Guid UnitId { get; set; }

		public IList<SelectListItem>? Categories { get; private set; }
		public IList<SelectListItem>? Units { get; private set; }

		public void SetCategoriesValues(IList<Category> categories)
		{
			Categories = RazorUtility.ConvertCategories(categories);
		}
		public void SetUnitValues(IList<Unit> units)
		{
			Units = RazorUtility.ConvertUnits(units);
		}
		public string AutoGenerateBarcode()
		{
			Guid myBarcodeGenerate = Guid.NewGuid();
			Barcode = myBarcodeGenerate.ToString().Replace("-", "").Substring(0, 10);
			return Barcode;
		}
	}
}
