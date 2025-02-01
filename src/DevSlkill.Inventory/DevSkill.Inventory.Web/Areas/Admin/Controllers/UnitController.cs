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
	public class UnitController : Controller
	{
		private readonly IUnitManagementService _unitManagementService;
		private readonly ILogger<UnitController> _logger;
		public UnitController(ILogger<UnitController> logger,
			IUnitManagementService unitManagementService)
		{
			_unitManagementService = unitManagementService;
			_logger = logger;
        }

       [ Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Index()
		{
			return View();
		}

		[HttpPost, Authorize(Roles = "Member,Admin,Support")]
		public JsonResult GetUnitJsonData([FromBody] UnitListModel model)
		{
			var result = _unitManagementService.
				GetUnits(model.PageIndex, model.PageSize, model.Search,
				model.FormatSortExpression("Name", "Id"));
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

		[Authorize(Roles = "Admin,Support")]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Create(UnitCreateModel model)
		{
			if (ModelState.IsValid)
			{
				var unit = new Unit
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				try
				{
					_unitManagementService.Create(unit);
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Unit create successfull.",
						Type = ResponseTypes.Success
					});
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Unit create failed.",
						Type = ResponseTypes.Danger
					});
					_logger.LogError(ex, "Unit create failed");
				}
			}

			return View(model);
		}

		[Authorize(Roles = "Admin,Support")]
		public IActionResult Update(Guid id)
		{
			var unit = _unitManagementService.GetUnit(id);
			var model = new UnitUpdateModel();
			model.Id = unit.Id;
			model.Name = unit.Name;
			return PartialView("_UnitUpdateModalPartial", model);
		}

		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Update(UnitUpdateModel model)
		{
			if (ModelState.IsValid)
			{
				var unit = new Unit
				{
					Id = model.Id,
					Name = model.Name
				};
				try
				{
					_unitManagementService.UpdateUnit(unit);
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Unit update successful.",
						Type = ResponseTypes.Success
					});
					return View("Index");
				}
				catch (Exception ex)
				{
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Unit update failed.",
						Type = ResponseTypes.Danger
					});
					_logger.LogError(ex, "Unit update failed");
				}
			}
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Delete(Guid Id)
		{
			try
			{
				_unitManagementService.DeleteUnit(Id);
				TempData.Put("ResponseMessage", new ResponseModel
				{
					Message = "Unit deleted successfull.",
					Type = ResponseTypes.Success
				});
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData.Put("ResponseMessage", new ResponseModel
				{
					Message = "Unit delete failed!",
					Type = ResponseTypes.Danger
				});
				_logger.LogError(ex, "Unit delete failed!");
			}
			return View();
		}
	}
}
