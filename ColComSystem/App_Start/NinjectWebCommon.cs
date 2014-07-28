[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ColComSystem.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ColComSystem.App_Start.NinjectWebCommon), "Stop")]


namespace ColComSystem.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http.Dependencies;
    using System.Collections.Generic;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Syntax;
    using Ninject.Web.Common;

    using CL.Interfaces;
    using CL.Service;

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
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDisposable>().To<Disposable>();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>();
            kernel.Bind(typeof(IRepository<>)).To(typeof(RepositoryBase<>));
            kernel.Bind<IHttpRuntimeWrapper>().To<HttpRuntimeWrapper>();
            kernel.Bind<INewsRepository>().To<NewsRepository>();
        }
    }

    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.GetAll(serviceType);
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }

        public new object GetService(Type serviceType)
        {
            return base.GetService(serviceType);
        }

        public new IEnumerable<object> GetServices(Type serviceType)
        {
            return base.GetServices(serviceType);
        }
    }
}