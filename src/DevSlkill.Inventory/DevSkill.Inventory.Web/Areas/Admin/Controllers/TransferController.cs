using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class TransferController : Controller
    {
        private readonly ITransferManagementService _transferManagementService;
        private readonly IStockManagementService _stockManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IProductManagementService _productManagementService;
        private readonly ILogger<TransferController> _logger;
        private readonly IMapper _mapper;
        public TransferController(ILogger<TransferController> logger,
            IMapper mapper, IStockManagementService stockManagementService,
            IWarehouseManagementService warehouseManagementService,
            IProductManagementService productManagementService,
            ITransferManagementService transferManagementService)
        {
            _stockManagementService = stockManagementService;
            _warehouseManagementService = warehouseManagementService;
            _productManagementService = productManagementService;
            _transferManagementService = transferManagementService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = "Member,Admin,Support")]
		public IActionResult Index()
		{
            var model = new TransferListModel();
            model.SetFromWarehouseValues(_warehouseManagementService.GetWarehouses());
            model.SetToWarehouseValues(_warehouseManagementService.GetWarehouses());
            model.SetProductValues(_productManagementService.GetProducts());
            return View(model);
		}


        [HttpPost, Authorize(Roles = "Member,Admin,Support")]
        public async Task<JsonResult> GetTransferJsonData([FromBody] TransferListModel model)
        {
            var result = await _transferManagementService.
                GetTransfer(model.PageIndex, model.PageSize, model.SearchItem,
                model.FormatSortExpression("Id", "ProductName"));
            var transferJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Date),
                                HttpUtility.HtmlEncode(record.ProductName),
                                HttpUtility.HtmlEncode(record.FromWarehouse),
                                HttpUtility.HtmlEncode(record.ToWarehouse),
                                HttpUtility.HtmlEncode(record.Quantity),
                                HttpUtility.HtmlEncode(record.Note),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(transferJsonData);
        }

        [HttpGet, Authorize(Roles = "Member,Admin,Support")]
        public async Task<IActionResult> GetStockQuantity(Guid productId, Guid warehouseId)
        {
            var stock = await _stockManagementService.GetStockInformationUsingProductAndWarehouseAsync(productId, warehouseId);
            if(stock == null)
            {
                return Json(new { stock = 0, sellPrice = 0 });
            }
            return Json(new { stock = stock.Quantity, sellPrice = stock.SellingPrice });
        }

        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Create()
        {
            var model = new TransferCreateModel();
            model.SetDateOnlyValues();
            model.SetProductValues(_productManagementService.GetProducts());
            model.SetFromWarehouseValues(_warehouseManagementService.GetWarehouses());
            model.SetToWarehouseValues(_warehouseManagementService.GetWarehouses());
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Member,Admin,Support")]
        public async Task <IActionResult> Create(TransferCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var transfer = _mapper.Map<StockTransfer>(model);
                transfer.Id = Guid.NewGuid();
                transfer.Product = _productManagementService.GetProduct(model.ProductId);
                transfer.FromWarehouse = _warehouseManagementService.GetWarehouse(model.FromWarehouseId);
                transfer.ToWarehouse = _warehouseManagementService.GetWarehouse(model.ToWarehouseId);
                var stockTransfer = await _stockManagementService.GetStockInformationUsingProductAndWarehouseAsync(model.ProductId, model.FromWarehouseId);
                stockTransfer.Quantity -= model.Quantity;
                if(stockTransfer.Quantity == 0)
                {
                    stockTransfer.SellingPrice = 0;
                    stockTransfer.PurchasePrice = 0;
                    stockTransfer.Reason = "Stock Out";
                }

                try
                {
                    _transferManagementService.CreateTransfer(transfer);
                    _stockManagementService.UpdateStock(stockTransfer);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Transfered successfuly",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Transfer failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Transfer failed");
                }
            }

            model.SetDateOnlyValues();
            model.SetFromWarehouseValues(_warehouseManagementService.GetWarehouses());
            model.SetToWarehouseValues(_warehouseManagementService.GetWarehouses());
            model.SetProductValues(_productManagementService.GetProducts());
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin,Support")]
        public IActionResult Delete(Guid Id)
        {
            try
            {
                _transferManagementService.DeleteTransfer(Id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Transfer deleted successfull.",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Transfer delete failed!",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Transfer delete failed!");
            }
            return View();
        }
    }
}
