using AutoMapper;
using Chatison.DataLayer;
using Chatison.DataLayer.Repositories;
using Chatison.Helpers;
using Chatison.Infrastructure.DataLayer;
using Chatison.Infrastructure.Managers;
using Chatison.Infrastructure.Repositories;
using Chatison.Managers;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Chatison
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.BindInRequestScope<DataContext>();

            container.BindInRequestScope<IUnitOfWork, UnitOfWork>();

            container.BindInRequestScope<IMobileProviderRepository, MobileProviderRepository>();
            container.BindInRequestScope<IGroupRepository, GroupRepository>();
            container.BindInRequestScope<IContactRepository, ContactRepository>();

            container.BindInRequestScope<IDataSeedManager, DataSeedManager>();
            container.BindInRequestScope<ISelectListManager, SelectListManager>();
            container.BindInRequestScope<IGroupManager, GroupManager>();
            container.BindInRequestScope<IContactManager, ContactManager>();

            var mappingConfig = new MapperConfiguration(options => { options.AddProfile(new Mapper.MapperProfile()); });

            var mapper = mappingConfig.CreateMapper();

            container.RegisterInstance<IMapper>(mapper);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}