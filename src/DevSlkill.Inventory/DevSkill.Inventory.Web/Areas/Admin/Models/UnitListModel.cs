using DevSkill.Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class UnitListModel : DataTables
	{
        public IList<SelectListItem>? Units { get; private set; }
    }
}
