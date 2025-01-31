using Microsoft.AspNetCore.Mvc.Rendering;
using REC.Inventory.Domain;
using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Infrastructure;

namespace REC.Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTables
    {
        public ProductSearchDto SearchItem { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }
        public IList<SelectListItem>? Units { get; private set; }

        public void SetUnitValues(IList<Unit> units)
        {
            Units = RazorUtility.ConvertUnits(units);
        }
        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }
    }
}
