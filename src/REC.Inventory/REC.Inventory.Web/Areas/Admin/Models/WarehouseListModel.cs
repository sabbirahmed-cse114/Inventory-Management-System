using Microsoft.AspNetCore.Mvc.Rendering;
using REC.Inventory.Domain;
using System.Data;

namespace REC.Inventory.Web.Areas.Admin.Models
{
    public class WarehouseListModel : DataTables
    {
        public IList<SelectListItem>? Warehouses { get; private set; }
    }
}
