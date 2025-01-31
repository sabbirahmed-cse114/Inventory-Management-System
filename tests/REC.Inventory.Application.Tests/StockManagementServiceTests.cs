using Autofac.Extras.Moq;
using Moq;
using REC.Inventory.Application.Services;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Domain.RepositoryContracts;
using System.Diagnostics.CodeAnalysis;


namespace REC.Inventory.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class StockManagementServiceTests
    {
        private AutoMock _moq;
        private IStockManagementService _stockManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IStockRepository> _stockRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _stockManagementService = _moq.Create<StockManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _stockRepositoryMock = _moq.Mock<IStockRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _stockRepositoryMock?.Reset();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _moq = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _moq?.Dispose();
        }

        [Test]
        public void CreateStock_GiveInformation_StockCreated()
        {
            Stock stock = new Stock();
            stock.Date = DateTime.Now;
            stock.Product = new Product { Name  = "Test" };
            stock.Warehouse = new Warehouse { Name = "Jahaj Company" };
            stock.Quantity = 1;
            stock.PurchasePrice = 200;
            stock.SellingPrice = 250;
            stock.Reason = "New Item";

            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository)
                .Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.Add(stock)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _stockManagementService.CreateStock(stock);

            _stockRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void DeleteStock_GiveStockId_StockDeleted()
        {
            Stock stock = new Stock();
            stock.Id = Guid.NewGuid();
            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository)
                .Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.Remove(stock.Id)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _stockManagementService.DeleteStock(stock.Id);

            _stockRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public async Task GetStockInformationAsync_ProvideStockId_ReturnStockInformation()
        {
            Stock stock = new Stock();
            stock.Id = Guid.NewGuid();
            stock.Product = new Product();
            stock.Product.Id = Guid.NewGuid();
            stock.Product.Name = "Chocolate";
            stock.Warehouse = new Warehouse();
            stock.Warehouse.Id = Guid.NewGuid();
            stock.Warehouse.Name = "Jahaj Company";
            stock.WantToProfit = 10;
            stock.Quantity = 300;
            stock.PurchasePrice = 90;
            stock.SellingPrice = 100;

            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository)
                    .Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.GetStockAsync(stock.Id))
                .ReturnsAsync(stock).Verifiable();

            var result = await _stockManagementService.GetStockInformationAsync(stock.Id);

            Assert.AreEqual(stock, result);
        }

        [Test]
        public async Task GetStockInformationUsingProductAndWarehouseAsync_ProvideProductIdAndWarehouseId_ReturnStockInformation()
        {
            var product = new Product();
            product.Id = Guid.NewGuid();
            var warehouse = new Warehouse();
            warehouse.Id = Guid.NewGuid();
            Stock stock = new Stock();
            stock.Id = Guid.NewGuid();
            stock.Product = new Product();
            stock.Product.Id = Guid.NewGuid();
            stock.Product.Name = "Chocolate";
            stock.Warehouse = new Warehouse();
            stock.Warehouse.Id = Guid.NewGuid();
            stock.Warehouse.Name = "Jahaj Company";
            stock.WantToProfit = 10;
            stock.Quantity = 300;
            stock.PurchasePrice = 90;
            stock.SellingPrice = 100;

            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository)
                    .Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.GetStockByProductAndWarehouseAsync(product.Id,warehouse.Id))
                .ReturnsAsync(stock).Verifiable();

            var result = await _stockManagementService.GetStockInformationUsingProductAndWarehouseAsync(product.Id,warehouse.Id);

            Assert.AreEqual(stock, result);
        }

        [Test]
        public void GetStock_GiveStockId_ReturnStockInfo()
        {
            Stock stock = new Stock();
            stock.Id = Guid.NewGuid();
            stock.Product = new Product();
            stock.Product.Id = Guid.NewGuid();
            stock.Product.Name = "Chocolate";
            stock.Warehouse = new Warehouse();
            stock.Warehouse.Id = Guid.NewGuid();
            stock.Warehouse.Name = "Jahaj Company";
            stock.WantToProfit = 10;
            stock.Quantity = 300;
            stock.PurchasePrice = 90;
            stock.SellingPrice = 100;
            stock.Reason = "Stock Out";
            stock.Note = "Hasan Tarik";

            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository)
                    .Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.GetById(stock.Id)).Returns(stock).Verifiable();

            var result = _stockManagementService.GetStock(stock.Id);

            Assert.AreEqual(stock, result);
        }

        [Test]
        public void UpdateStock_ProvideStockInformation_StockUpdated()
        {
            Stock stock = new Stock();
            stock.Id = Guid.NewGuid();
            stock.Product = new Product();
            stock.Product.Id = Guid.NewGuid();
            stock.Warehouse = new Warehouse();
            stock.Warehouse.Id = Guid.NewGuid();
            stock.WantToProfit = 10;
            stock.Quantity = 300;
            stock.PurchasePrice = 90;
            stock.SellingPrice = 100;

            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository)
                .Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.Edit(stock)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _stockManagementService.UpdateStock(stock);

            _stockRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }
    }
}
