using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class WarehouseManagementServiceTests
    {
        private AutoMock _moq;
        private IWarehouseManagementService _warehouseManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IWarehouseRepository> _warehouseRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _warehouseManagementService = _moq.Create<WarehouseManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _warehouseRepositoryMock = _moq.Mock<IWarehouseRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _warehouseRepositoryMock?.Reset();
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
        public void CreateWarehouse_NameNotDuplicate_WarehouseCreated()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Name = "Jahaj Company";

            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.IsTitleDuplicate(warehouse.Name, null))
                .Returns(false);
            _warehouseRepositoryMock.Setup(x => x.Add(warehouse)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _warehouseManagementService.CreateWarehouse(warehouse);

            _warehouseRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void CreateWarehouse_NameDuplicate_WarehouseCreateFailed()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Name = "Jahaj Company";

            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.IsTitleDuplicate(warehouse.Name,null))
                .Returns(true);

            var error = Assert.Throws<InvalidOperationException>(() =>
            _warehouseManagementService.CreateWarehouse(warehouse));

            Assert.AreEqual("Warehouse Name should be unique.", error?.Message);
        }

        [Test]
        public void DeleteWarehouse_GiveWarehouseId_DeleteWarehouse()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Id = Guid.NewGuid();
            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.Remove(warehouse.Id)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _warehouseManagementService.DeleteWarehouse(warehouse.Id);

            _warehouseRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void GetWarehouses_ReturnAllWarehouse()
        {
            var warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse { Id = Guid.NewGuid(), Name = "Jahaj Company" });
            warehouses.Add(new Warehouse { Id = Guid.NewGuid(), Name = "Medical Mor" });
            warehouses.Add(new Warehouse { Id = Guid.NewGuid(), Name = "Modern Mor" });

            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                    .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.GetAll())
                    .Returns(warehouses).Verifiable();

            var result = _warehouseManagementService.GetWarehouses();

            Assert.AreEqual(result, warehouses);
        }

        [Test]
        public void UpdateWarehouse_NameNotDuplicate_WarehouseUpdated()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Id = Guid.NewGuid();
            warehouse.Name = "Modern Mor";

            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.IsTitleDuplicate(warehouse.Name, warehouse.Id))
                .Returns(false);
            _warehouseRepositoryMock.Setup(x => x.Edit(warehouse)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _warehouseManagementService.UpdateWarehouse(warehouse);

            _warehouseRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void UpdateWarehouse_DuplicateName_WarehouseUpdateFailed()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Id = Guid.NewGuid();
            warehouse.Name = "Test";

            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.IsTitleDuplicate(warehouse.Name, warehouse.Id))
                .Returns(true);

            var error = Assert.Throws<InvalidOperationException>(() =>
            _warehouseManagementService.UpdateWarehouse(warehouse));

            Assert.AreEqual("Warehouse Name should be unique.", error?.Message);
        }

        [Test]
        public async Task GetWarehouseInformationAsync_WarehouseIdProvide_ReturnWarehouseInformation()
        {
            Warehouse warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "Jahaj Company"
            };

            _inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository)
                    .Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(x => x.GetWarehouseAsync(warehouse.Id))
                .ReturnsAsync(warehouse).Verifiable();

            var result = await _warehouseManagementService.GetWarehouseInformationAsync(warehouse.Id);

            Assert.AreEqual(warehouse, result);
        }
    }
}
