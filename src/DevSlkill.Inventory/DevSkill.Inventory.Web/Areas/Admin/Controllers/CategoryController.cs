using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly ILogger<UnitController> _logger;
        public CategoryController(ILogger<UnitController> logger,
            ICategoryManagementService categoryManagementService)
        {
            _categoryManagementService = categoryManagementService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = "Member,Admin,Support")]
        public JsonResult GetCategoryJsonData([FromBody] CategoryListModel model)
        {
            var result = _categoryManagementService.
                GetCategories(model.PageIndex, model.PageSize, model.Search,
                model.FormatSortExpression("Name"));
            var unitJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(unitJsonData);
        }

		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Create(CategoryCreateModel model)
		{
			if (ModelState.IsValid)
			{
				var category = new Category
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				try
				{
					_categoryManagementService.Create(category);
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Category create successfull.",
						Type = ResponseTypes.Success
					});
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Category create failed.",
						Type = ResponseTypes.Danger
					});
					_logger.LogError(ex, "Category create failed");
				}
			}

			return View(model);
        }


        [Authorize(Roles = "Admin,Support")]
        public IActionResult Update(Guid id)
		{
			var category = _categoryManagementService.GetCategory(id);
			var model = new CategoryUpdateModel
			{
				Id = category.Id,
				Name = category.Name
			};
			return PartialView("_CategoryUpdateModalPartial", model);
		}

		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Update(CategoryUpdateModel model)
		{
			if (ModelState.IsValid)
			{
				var category = new Category
				{
					Id = model.Id,
					Name = model.Name
				};
				try
				{
					_categoryManagementService.UpdateCategory(category);
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Category update successful.",
						Type = ResponseTypes.Success
					});
					return View("Index");
				}
				catch (Exception ex)
				{
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Category update failed.",
						Type = ResponseTypes.Danger
					});
					_logger.LogError(ex, "Category update failed");
				}
			}
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Delete(Guid Id)
		{
			try
			{
				_categoryManagementService.DeleteCategory(Id);
				TempData.Put("ResponseMessage", new ResponseModel
				{
					Message = "Category deleted successfull.",
					Type = ResponseTypes.Success
				});
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData.Put("ResponseMessage", new ResponseModel
				{
					Message = "Category delete failed!",
					Type = ResponseTypes.Danger
				});
				_logger.LogError(ex, "Category delete failed!");
			}
			return View();
		}
	}
}
