using DataAccess;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SklepInternetowy.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(SklepInternetowy.App_Start.NinjectWebCommon), "Stop")]

namespace SklepInternetowy.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<INhibernateConnection>().To<NHibernateConnection>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IManufacturersRepository>().To<ManufacturersRepository>();
            kernel.Bind<IProductsRepository>().To<ProductsRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IProductImagesRepository>().To<ProductImagesRepository>();
            kernel.Bind<IOrdersRepository>().To<OrdersRepository>();
            kernel.Bind<IOrderDetailsRepository>().To<OrderDetailsRepository>();
            kernel.Bind<IShoppingCartRepository>().To<ShoppingCartRepository>();
            kernel.Bind<IDeliveryTypesRepository>().To<DeliveryTypesRepository>();
        }
    }
}
