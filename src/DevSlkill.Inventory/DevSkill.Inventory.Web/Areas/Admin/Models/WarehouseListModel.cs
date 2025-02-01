using DevSkill.Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class WarehouseListModel : DataTables
    {
        public IList<SelectListItem>? Warehouses { get; private set; }
    }
}
