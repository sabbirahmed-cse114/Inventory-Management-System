using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class ProductUpdateModel
	{
		public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Barcode { get; set; }

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
