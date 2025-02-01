using DevSkill.Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class CategoryListModel : DataTables
    {
        public IList<SelectListItem>? Categories { get; private set; }
    }
}
