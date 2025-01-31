using Autofac.Extras.Moq;
using REC.Inventory.Application.Services;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Domain.RepositoryContracts;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace REC.Inventory.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class CategoryManagementServiceTests
    {
        private AutoMock _moq;
        private ICategoryManagementService _categoryManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _categoryManagementService = _moq.Create<CategoryManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _categoryRepositoryMock = _moq.Mock<ICategoryRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _categoryRepositoryMock?.Reset();
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
        public void GetCategories_ProvideAllCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category { Id = Guid.NewGuid(), Name = "General" });
            categories.Add(new Category { Id = Guid.NewGuid(), Name = "Food" });
            categories.Add(new Category { Id = Guid.NewGuid(), Name = "Test" });

            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.GetAll())
                .Returns(categories).Verifiable();

            var result = _categoryManagementService.GetCategories();

            //Assert.AreSame(categories, result);
            Assert.AreEqual(result, categories);
        }

        [Test]
        public void GetCategory_ProvideCategoryId_ReturnCategory()
        {
            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };

            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.GetById(category.Id))
                .Returns(category).Verifiable();

            var result = _categoryManagementService.GetCategory(category.Id);

            Assert.AreEqual(category, result);
        }

        [Test]
        public void CreateCategory_NameNotDuplicate_CategoryCreated()
        {
            Category category = new Category();
            category.Name = "General";

            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.IsTitleDuplicate(category.Name, null))
                .Returns(false);
            _categoryRepositoryMock.Setup(x => x.Add(category)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _categoryManagementService.Create(category);

            _categoryRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void CreateCategory_NameDuplicate_CategoryCreateFailed()
        {
            Category category = new Category();
            category.Name = "Test";

            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.IsTitleDuplicate(category.Name, null))
                .Returns(true);

            var error = Assert.Throws<InvalidOperationException>(() =>
            _categoryManagementService.Create(category));

            Assert.AreEqual("Category should be unique.", error?.Message);
        }

        [Test]
        public void UpdateCategory_NameNotDuplicate_CategoryUpdated()
        {
            Category category = new Category();
            category.Id = Guid.NewGuid();
            category.Name = "General";

            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.IsTitleDuplicate(category.Name, category.Id))
                .Returns(false);
            _categoryRepositoryMock.Setup(x => x.Edit(category)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _categoryManagementService.UpdateCategory(category);

            _categoryRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

        [Test]
        public void UpdateCategory_NameDuplicate_CategoryUpdateFailed()
        {
            Category category = new Category();
            category.Id = Guid.NewGuid();
            category.Name = "Test";

            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.IsTitleDuplicate(category.Name, category.Id))
                .Returns(true);

            var error = Assert.Throws<InvalidOperationException>(() =>
            _categoryManagementService.UpdateCategory(category));

            Assert.AreEqual("Category should be unique.", error?.Message);
        }

        [Test]
        public void DeleteCategory_ProvideCategoryId_CategoryDeleted()
        {
            Category category = new Category();
            category.Id = Guid.NewGuid();
            _inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object);
            _categoryRepositoryMock.Setup(x => x.Remove(category.Id)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _categoryManagementService.DeleteCategory(category.Id);

            _categoryRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }
    }
}