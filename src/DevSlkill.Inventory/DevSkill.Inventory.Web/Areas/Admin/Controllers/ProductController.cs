using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using AutoMapper;
using DevSkill.Inventory.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IUnitManagementService _unitManagementService;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        public ProductController(ILogger<ProductController> logger,
            IMapper mapper, IProductManagementService productManagementService,
            ICategoryManagementService categoryManagementService,
            IUnitManagementService unitManagementService)
        {
            _productManagementService = productManagementService;
            _categoryManagementService = categoryManagementService;
            _unitManagementService = unitManagementService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet,Authorize(Roles = "Admin,Support")]
        public IActionResult GenerateBarcode()
        {
            var model = new ProductCreateModel();
            var barcode = model.AutoGenerateBarcode();
            return Content(barcode);
        }


        [Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Index()
        {
            var model = new ProductListModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            model.SetUnitValues(_unitManagementService.GetUnits());
            return View(model);
        }

        [HttpPost,Authorize(Roles = "Member,Admin,Support")]
        public async Task<JsonResult> GetProductJsonData([FromBody] ProductListModel model)
        {
            var result = await _productManagementService.
                GetProductsSP(model.PageIndex, model.PageSize, model.SearchItem,
                model.FormatSortExpression("Id", "Name", "Barcode", "Status","CategoryId"));
            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.ImagePath),
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Barcode),
                                HttpUtility.HtmlEncode(record.Status),
                                HttpUtility.HtmlEncode(record.Category),
                                HttpUtility.HtmlEncode(record.Unit),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(productJsonData);
        }

        [Authorize(Roles = "Admin,Support")]
        public IActionResult Create()
        {
            var model = new ProductCreateModel();
            model.SetCategoriesValues(_categoryManagementService.GetCategories());
            model.SetUnitValues(_unitManagementService.GetUnits());
            return View(model);
        }
        [HttpPost, RequestSizeLimit(104857600), ValidateAntiForgeryToken,Authorize(Roles = "Member,Admin,Support")]
        public IActionResult Create(ProductCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".PNG" };
                string? uniqueFileName = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    string fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid image format. Only JPG, JPEG, PNG, and GIF are allowed.");
                        return View(model);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }
                }

                var product = _mapper.Map<Product>(model);
                product.Id = Guid.NewGuid();
                product.ImagePath = uniqueFileName;
                product.Category = _categoryManagementService.GetCategory(model.CategoryId);
                product.Unit = _unitManagementService.GetUnit(model.UnitId);

                try
                {
                    _productManagementService.CreateProduct(product);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product created successfuly",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product creation failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Product creation failed");
                }
            }
            model.SetCategoriesValues(_categoryManagementService.GetCategories());
            model.SetUnitValues(_unitManagementService.GetUnits());
            return View(model);
        }

        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> Update(Guid id)
        {
            Product product = await _productManagementService.GetProductInformationAsync(id);
            var model = _mapper.Map<ProductUpdateModel>(product);
            model.SetCategoriesValues(_categoryManagementService.GetCategories());
            model.SetUnitValues(_unitManagementService.GetUnits());
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken,Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await _productManagementService.GetProductInformationAsync(model.Id);
                product = _mapper.Map(model, product);
                try
                {
                    _productManagementService.UpdateProduct(product);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product update successfull.",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product update failed.",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product update failed");
                }
            }
            model.SetCategoriesValues(_categoryManagementService.GetCategories());
            model.SetUnitValues(_unitManagementService.GetUnits());
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken,Authorize(Roles = "Admin,Support")]
        public IActionResult Delete(Guid Id)
        {
            try
            {
                _productManagementService.DeleteProduct(Id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Product deleted successfull.",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Product delete failed!",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Product delete failed!");
            }
            return View();
        }
    }
}
