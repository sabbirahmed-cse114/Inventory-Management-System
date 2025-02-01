using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class RazorUtility
    {
        public static IList<SelectListItem> ConvertCategories(IList<Category> categories)
        {
            var items = (from c in categories
                         select new SelectListItem(c.Name, c.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select Chemical Type", string.Empty));

            return items;
        }
        public static IList<SelectListItem> ConvertUnits(IList<Unit> units)
        {
            var items = (from u in units
                         select new SelectListItem(u.Name, u.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select Unit", string.Empty));

            return items;
        }
        public static IList<SelectListItem> ConvertWarehouses(IList<Warehouse> warehouses)
        {
            var items = (from w in warehouses
                         select new SelectListItem(w.Name, w.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select Warehouse", string.Empty));

            return items;
        }
        public static IList<SelectListItem> ConvertProducts(IList<Product> products)
        {
            var items = (from p in products
                         select new SelectListItem(p.Name, p.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select Product", string.Empty));

            return items;
        }
        public static DateTime ConvertDateOnly()
        {
            var today = DateTime.Now;
            return today;
        }
    }
}
