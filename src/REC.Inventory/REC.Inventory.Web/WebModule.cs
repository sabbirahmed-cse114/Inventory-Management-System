using Autofac;
using REC.Inventory.Application;
using REC.Inventory.Application.Services;
using REC.Inventory.Domain.RepositoryContracts;
using REC.Inventory.Infrastructure;
using REC.Inventory.Infrastructure.Repositories;
using REC.Inventory.Infrastructure.UnitOfWorks;
using REC.Inventory.Web.Data;
using REC.Inventory.Web.Models;

namespace REC.Inventory.Web
{
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Membership>().As<IMembership>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailSender>().As<IEmailSender>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InventoryUnitOfWork>().
                As<IInventoryUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductManagementService>().
                As<IProductManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitManagementService>().
                As<IUnitManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CategoryManagementService>().
                As<ICategoryManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WarehouseManagementService>().
                As<IWarehouseManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<StockManagementService>().
                As<IStockManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TransferManagementService>().
                As<ITransferManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>().
                As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitRepository>().
                As<IUnitRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryRepository>().
                As<ICategoryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WarehouseRepository>().
                As<IWarehouseRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<StockRepository>().
                As<IStockRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TransferRepository>().
                As<ITransferRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<InventoryDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();
        }
    }
}
