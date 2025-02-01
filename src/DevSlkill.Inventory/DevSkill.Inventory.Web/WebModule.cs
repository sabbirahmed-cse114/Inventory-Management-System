using Autofac;
using AutoMapper;
using DevSkill.Inventory.Application;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using DevSkill.Inventory.Web.Data;
using DevSkill.Inventory.Web.Models;

namespace DevSkill.Inventory.Web
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
