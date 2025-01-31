using AutoMapper;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Web.Areas.Admin.Models;

namespace REC.Inventory.Web
{
	public class WebProfile : Profile
	{
		public WebProfile()
		{
			CreateMap<ProductCreateModel, Product>().ReverseMap();
            CreateMap<ProductUpdateModel, Product>().ReverseMap();
            CreateMap<StockCreateModel, Stock>().ReverseMap();
			CreateMap<StockUpdateModel, Stock>().ReverseMap();
			CreateMap<TransferCreateModel,StockTransfer>().ReverseMap();
        }
	}
}
