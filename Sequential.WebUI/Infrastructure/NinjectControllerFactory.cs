using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Concrete;

namespace Sequential2013.WebUI.Infrastructure
{
   public class NinjectControllerFactory : DefaultControllerFactory
   {
      //Ninject kernel is the thing that can supply object instances.
      private IKernel kernel = new StandardKernel(new SequentialServices());

      //ASP.NET MVC calls this to get the controller for each request.
      protected override IController GetControllerInstance(
         RequestContext requestContext, Type controllerType) 
      {
         if (controllerType == null) return null;
         return (IController)kernel.Get(controllerType);   
      }

      //Configures how abstract service types are mapped to concrete impl.
      private class SequentialServices : NinjectModule
      {
         public override void Load()
         {
            Bind<ISeqPostsRepository>()
               .To<SeqPostsRepository>()
               .WithConstructorArgument("connectionString",
                  ConfigurationManager.ConnectionStrings["AppDb"]
                                      .ConnectionString);
				Bind<ISeqCategoriesRepository>()
					.To<SeqCategoriesRepository>()
					.WithConstructorArgument("connectionString",
						ConfigurationManager.ConnectionStrings["AppDb"]
												  .ConnectionString);
				Bind<ISeqTagsRepository>()
					.To<SeqTagsRepository>()
					.WithConstructorArgument("connectionString",
						ConfigurationManager.ConnectionStrings["AppDb"]
												  .ConnectionString);
				//Bind<ISeqBooksRepository>()
				//   .To<SeqBooksRepository>()
				//   .WithConstructorArgument("connectionString",
				//      ConfigurationManager.ConnectionStrings["AppDb"]
				//                          .ConnectionString);
         }
      }
   } //end class
}