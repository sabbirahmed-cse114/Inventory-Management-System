using Microsoft.AspNetCore.Mvc.Rendering;
using REC.Inventory.Domain;

namespace REC.Inventory.Web.Areas.Admin.Models
{
    public class CategoryListModel : DataTables
    {
        public IList<SelectListItem>? Categories { get; private set; }
    }
}
