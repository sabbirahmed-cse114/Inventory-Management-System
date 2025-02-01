using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly ILogger<WarehouseController> _logger;
        public WarehouseController(ILogger<WarehouseController> logger,
            IWarehouseManagementService warehouseManagementService)
        {
            _warehouseManagementService = warehouseManagementService;
            _logger = logger;
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = "Member,Admin,Support")]
        public JsonResult GetWarehouseJsonData([FromBody] WarehouseListModel model)
        {
            var result = _warehouseManagementService.
                GetWarehouses(model.PageIndex, model.PageSize, model.Search,
                model.FormatSortExpression("Name"));
            var warehouseJsonData = new
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
            return Json(warehouseJsonData);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
        public IActionResult Create(WarehouseCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouse = new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                };

                try
                {
                    _warehouseManagementService.CreateWarehouse(warehouse);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse create successfull.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehousey create failed.",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Warehouse create failed");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin,Support")]
        public IActionResult Update(Guid id)
        {
            var warehouse = _warehouseManagementService.GetWarehouse(id);
            var model = new WarehouseUpdateModel
            {
                Id = warehouse.Id,
                Name = warehouse.Name
            };
            return PartialView("_WarehouseUpdateModalPartial", model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
        public IActionResult Update(WarehouseUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouse = new Warehouse
                {
                    Id = model.Id,
                    Name = model.Name
                };
                try
                {
                    _warehouseManagementService.UpdateWarehouse(warehouse);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse update successful.",
                        Type = ResponseTypes.Success
                    });
                    return View("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse update failed.",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Warehouse update failed");
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
        public IActionResult Delete(Guid Id)
        {
            try
            {
                _warehouseManagementService.DeleteWarehouse(Id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Warehouse deleted successfull.",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Warehouse delete failed!",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Warehouse delete failed!");
            }
            return View();
        }
    }
}
