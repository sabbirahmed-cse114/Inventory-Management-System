using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class ProductManagementServiceTests
    {
        private AutoMock _moq;
        private IProductManagementService _productManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IProductRepository> _productRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _productManagementService = _moq.Create<ProductManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _productRepositoryMock = _moq.Mock<IProductRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _productRepositoryMock?.Reset();
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
        public void CreateProduct_NameNotDuplicate_ProductCreated()
        {
            Product product = new Product();
            product.Name = "Chocolate";
            product.ImagePath = "dkjddflshrfs.jpg";
            product.Barcode = "000000018";
            product.Status = "Active";
            product.Category = new Category { Name = "General" };
            product.Unit = new Unit { Name = "Inch" };


            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.IsTitleDuplicate(product.Name, null))
                .Returns(false);
            _productRepositoryMock.Setup(x => x.Add(product)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _productManagementService.CreateProduct(product);

            _productRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void CreateProduct_DuplicateName_ProductCreateFailed()
        {
            Product product = new Product();
            product.Name = "Test";
            product.ImagePath = "abcdkdi.jpg";
            product.Barcode = "00002984";
            product.Status = "Active";
            product.Category = new Category { Name = "General" };
            product.Unit = new Unit { Name = "Inch" };

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.IsTitleDuplicate(product.Name, null))
                .Returns(true);

            var error = Assert.Throws<InvalidOperationException>(() =>
            _productManagementService.CreateProduct(product));

            Assert.AreEqual("Product name should be unique.", error?.Message);
        }

        [Test]
        public void UpdateProduct_NameNotDuplicate_ProductUpdated()
        {
            Product product = new Product();
            product.Id = Guid.NewGuid();
            product.Name = "Test";
            product.ImagePath = "abcdefgh.jpg";
            product.Barcode = "0000112234";
            product.Status = "Inactive";
            product.Category = new Category { Name = "Food" };
            product.Unit = new Unit { Name = "KG" };

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.IsTitleDuplicate(product.Name, product.Id))
                .Returns(false);
            _productRepositoryMock.Setup(x => x.Edit(product)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _productManagementService.UpdateProduct(product);

            _productRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void UpdateProduct_DuplicateName_ProductUpdateFailed()
        {
            Product product = new Product();
            product.Id = Guid.NewGuid();
            product.Name = "Test";
            product.ImagePath = "abcdefgh.jpg";
            product.Barcode = "0000112234";
            product.Status = "Inactive";
            product.Category = new Category { Name = "Food" };
            product.Unit = new Unit { Name = "KG" };

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.IsTitleDuplicate(product.Name, product.Id))
                .Returns(true);

            var error = Assert.Throws<InvalidOperationException>(() =>
            _productManagementService.UpdateProduct(product));

            Assert.AreEqual("Name should be unique.", error?.Message);
        }

        [Test]
        public void DeleteProduct_ProductIdProvided_ProductDeleted()
        {
            Product product = new Product();
            product.Id = Guid.NewGuid();
            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.Remove(product.Id)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _productManagementService.DeleteProduct(product.Id);

            _productRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public async Task GetProductInformationAsync_ProductIdProvided_ReturnProductInformation()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                ImagePath = "abcdefgh.jpg",
                Barcode = "0000015",
                Status = "Active",
                Category = new Category { Name = "Test" },
                Unit = new Unit { Name = "Test" }
            };

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetProductAsync(product.Id))
                .ReturnsAsync(product).Verifiable();

            var result = await _productManagementService.GetProductInformationAsync(product.Id);

            Assert.AreEqual(product, result);            
        }


        [Test]
        public void GetProducts_ReturnAllProducts()
        {
            var products = new List<Product>();
            products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Soap",
                ImagePath = "hfjkdhuf.jpg",
                Barcode = "0000012",
                Status = "Active",
                Category = new Category { Name = "Test" },
                Unit = new Unit { Name = "Test" }
            });

            products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Car",
                ImagePath = "hfitjfkdhuf.jpg",
                Barcode = "0000013",
                Status = "Active",
                Category = new Category { Name = "Food" },
                Unit = new Unit { Name = "kg" }
            });

            products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Chocolate",
                ImagePath = "hjldusngdhuf.jpg",
                Barcode = "0000016",
                Status = "Active",
                Category = new Category { Name="Kids Item" },
                Unit = new Unit { Name = "Piece" }
            });

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.GetAll())
                .Returns(products).Verifiable();

            var result = _productManagementService.GetProducts();

            Assert.AreEqual(result, products);
        }
    }
}