using Microsoft.AspNetCore.Mvc;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using REC.Inventory.Application.Services;
using REC.Inventory.Web.Areas.Admin.Models;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Infrastructure;

namespace REC.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
	public class StockController : Controller
	{
		private readonly IStockManagementService _stockManagementService;
		private readonly IWarehouseManagementService _warehouseManagementService;
		private readonly IProductManagementService _productManagementService;
		private readonly ILogger<StockController> _logger;
		private readonly IMapper _mapper;
		public StockController(ILogger<StockController> logger,
			IMapper mapper, IStockManagementService stockManagementService,
			IWarehouseManagementService warehouseManagementService,
			IProductManagementService productManagementService)
		{
			_stockManagementService = stockManagementService;
			_warehouseManagementService = warehouseManagementService;
			_productManagementService = productManagementService;
			_logger = logger;
			_mapper = mapper;
		}

		[Authorize(Roles = "Member,Admin,Support")]
		public IActionResult Index()
		{
			var model = new StockListModel();
			model.SetWarehouseValues(_warehouseManagementService.GetWarehouses());
			model.SetProductValues(_productManagementService.GetProducts());
			return View(model);
		}

		[HttpPost, Authorize(Roles = "Member,Admin,Support")]
		public async Task<JsonResult> GetStockJsonData([FromBody] StockListModel model)
		{
			var result = await _stockManagementService.
				GetStocksSP(model.PageIndex, model.PageSize, model.SearchItem,
				model.FormatSortExpression("Id","Product","Warehouse","Quantity","Reason"));
			var stockJsonData = new
			{
				recordsTotal = result.total,
				recordsFiltered = result.totalDisplay,
				data = (from record in result.data
						select new string[]
						{
								HttpUtility.HtmlEncode(record.Date),
								HttpUtility.HtmlEncode(record.Product),
								HttpUtility.HtmlEncode(record.Warehouse),
								HttpUtility.HtmlEncode(record.Quantity),
								HttpUtility.HtmlEncode(record.PurchasePrice),
								HttpUtility.HtmlEncode(record.SellingPrice),
                                HttpUtility.HtmlEncode(record.Reason),
								HttpUtility.HtmlEncode(record.Note),
								record.Id.ToString()
						}
					).ToArray()
			};
			return Json(stockJsonData);
        }

        [Authorize(Roles = "Admin,Support")]
        public IActionResult Create()
		{
			var model = new StockCreateModel();
			model.SetDateOnlyValues();
			model.SetProductValues(_productManagementService.GetProducts());
			model.SetWarehouseValues(_warehouseManagementService.GetWarehouses());
			return View(model);
		}
		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Create(StockCreateModel model)
		{
            if (ModelState.IsValid)
            {
                var stock = _mapper.Map<Stock>(model);
				stock.Id = Guid.NewGuid();
				stock.SellingPrice = model.ProfitCalculation(model.Profit, model.PurchasePrice);
				stock.Reason = "New item";
				stock.WantToProfit = model.Profit;
				stock.Product = _productManagementService.GetProduct(model.ProductId);
				stock.Warehouse = _warehouseManagementService.GetWarehouse(model.WarehouseId);

				try
				{
					_stockManagementService.CreateStock(stock);

					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Stock added successfuly",
						Type = ResponseTypes.Success
					});

					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Stock added failed",
						Type = ResponseTypes.Danger
					});

					_logger.LogError(ex, "Stock added failed");
				}
			}

			model.SetDateOnlyValues();
			model.SetProductValues(_productManagementService.GetProducts());
			model.SetWarehouseValues(_warehouseManagementService.GetWarehouses());
			return View(model);
		}

		[Authorize(Roles = "Admin,Support")]
		public async Task<IActionResult> Update(Guid id)
		{
			var stock = await _stockManagementService.GetStockInformationAsync(id);
            var model = _mapper.Map<StockUpdateModel>(stock);
            var product = await _productManagementService.GetProductInformationAsync(model.ProductId);
			var warehouse = await _warehouseManagementService.GetWarehouseInformationAsync(model.WarehouseId);
            model.ProductName = product.Name;
			model.WarehouseName = warehouse.Name;
			model.SellingPrice = stock.SellingPrice;
			model.Profit = stock.WantToProfit;
			model.SetDateOnlyValues();
            return PartialView("_StockUpdateModalPartial",model);
		}


		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public async Task<IActionResult> Update(StockUpdateModel model)
		{
			if (ModelState.IsValid)
			{
                var stock = await _stockManagementService.GetStockInformationAsync(model.Id);
                stock = _mapper.Map(model, stock);
				stock.Quantity = stock.Quantity + model.AddStock;
				if(stock.Quantity == 0)
				{
					stock.PurchasePrice = 0;
					stock.SellingPrice = 0;
				}
				else
				{
                    stock.PurchasePrice = model.CurrentPriceCalculation();
                    stock.SellingPrice = model.ProfitCalculation();
                }             
				
				_stockManagementService.UpdateStock(stock);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Stock added successfuly",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
			}
			return PartialView("_StockUpdateModalPartial", model);
		}


		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
		public IActionResult Delete(Guid Id)
		{
			try
			{
				_stockManagementService.DeleteStock(Id);
				TempData.Put("ResponseMessage", new ResponseModel
				{
					Message = "Stock deleted successfull.",
					Type = ResponseTypes.Success
				});
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData.Put("ResponseMessage", new ResponseModel
				{
					Message = "Stock delete failed!",
					Type = ResponseTypes.Danger
				});
				_logger.LogError(ex, "Stock delete failed!");
			}
			return View();
		}
	}
}
