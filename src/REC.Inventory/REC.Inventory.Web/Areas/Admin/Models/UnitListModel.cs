using REC.Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace REC.Inventory.Web.Areas.Admin.Models
{
	public class UnitListModel : DataTables
	{
        public IList<SelectListItem>? Units { get; private set; }
    }
}
