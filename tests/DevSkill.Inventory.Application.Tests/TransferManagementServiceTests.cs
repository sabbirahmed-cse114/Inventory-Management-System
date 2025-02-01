using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class TransferManagementServiceTests
    {
        private AutoMock _moq;
        private ITransferManagementService _transferManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<ITransferRepository> _transferRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _transferManagementService = _moq.Create<TransferManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _transferRepositoryMock = _moq.Mock<ITransferRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _transferRepositoryMock?.Reset();
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
        public void CreateTransfer_ProvideTransferInformation_TransferCreated()
        {
            StockTransfer transfer = new StockTransfer();
            transfer.Id = Guid.NewGuid();
            transfer.FromWarehouse = new Warehouse();
            transfer.FromWarehouse.Id = Guid.NewGuid();
            transfer.ToWarehouse = new Warehouse();
            transfer.ToWarehouse.Id = Guid.NewGuid();
            transfer.Quantity = 1;


            _inventoryUnitOfWorkMock.Setup(x => x.TransferRepository)
                .Returns(_transferRepositoryMock.Object);
            _transferRepositoryMock.Setup(x => x.Add(transfer)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _transferManagementService.CreateTransfer(transfer);

            _transferRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void DeleteTransfer_ProvideTransferId_TransferDeleted()
        {
            StockTransfer transfer = new StockTransfer();
            transfer.Id = Guid.NewGuid();
            _inventoryUnitOfWorkMock.Setup(x => x.TransferRepository)
                .Returns(_transferRepositoryMock.Object);
            _transferRepositoryMock.Setup(x => x.Remove(transfer.Id)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _transferManagementService.DeleteTransfer(transfer.Id);

            _transferRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetTransfer_ProvideTransferId_ReturnTransferInformation()
        {
            StockTransfer transfer = new StockTransfer();
            transfer.Id = Guid.NewGuid();
            transfer.Product = new Product();
            transfer.Product.Id = Guid.NewGuid();
            transfer.FromWarehouse = new Warehouse();
            transfer.FromWarehouse.Id = Guid.NewGuid();
            transfer.ToWarehouse = new Warehouse();
            transfer.ToWarehouse.Id = Guid.NewGuid();

            _inventoryUnitOfWorkMock.Setup(x => x.TransferRepository)
                    .Returns(_transferRepositoryMock.Object);
            _transferRepositoryMock.Setup(x => x.GetById(transfer.Id))
                .Returns(transfer).Verifiable();

            var result = _transferManagementService.GetTransfer(transfer.Id);

            Assert.AreEqual(transfer, result);
        }


        [Test]
        public async Task GetTransferInformationAsync_TransferIdProvided_ReturnTransferInformation()
        {
            StockTransfer transfer = new StockTransfer();
            transfer.Id = Guid.NewGuid();
            transfer.Product = new Product();
            transfer.Product.Id = Guid.NewGuid();
            transfer.FromWarehouse = new Warehouse();
            transfer.FromWarehouse.Id = Guid.NewGuid();
            transfer.ToWarehouse= new Warehouse();
            transfer.ToWarehouse.Id = Guid.NewGuid();
            transfer.Quantity = 200;
            transfer.Note = "Sales";

            _inventoryUnitOfWorkMock.Setup(x => x.TransferRepository)
                    .Returns(_transferRepositoryMock.Object);
            _transferRepositoryMock.Setup(x => x.GetTransferAsync(transfer.Id))
                .ReturnsAsync(transfer).Verifiable();

            var result = await _transferManagementService.GetTransferInformationAsync(transfer.Id);

            Assert.AreEqual(transfer, result);
        }

    }
}
