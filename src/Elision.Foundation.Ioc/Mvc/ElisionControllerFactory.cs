using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Diagnostics;
using Sitecore.Mvc.Helpers;

namespace Elision.Foundation.Ioc.Mvc
{
    public class ElisionControllerFactory : DefaultControllerFactory
    {
        private readonly Dictionary<string, XmlNode> _knownTypes;
        private readonly ElisionObjectFactory _objectFactory;
        private readonly IControllerFactory _innerFactory;

        public ElisionControllerFactory(IControllerFactory innerFactory)
        {
            _innerFactory = innerFactory;
            _knownTypes = new Dictionary<string, XmlNode>();
            _objectFactory = new ElisionObjectFactory();
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return CreateControllerInstance(requestContext, controllerName);
        }

        protected IController CreateControllerInstance(RequestContext requestContext, string controllerName)
        {
            Assert.ArgumentNotNull(requestContext, "requestContext");

            HttpException httpException = null;
            IController controller = null;
            try
            {
                controller = _innerFactory?.CreateController(requestContext, controllerName);
            }
            catch (HttpException ex) // would be thrown by the default MVC controller factory
            {
                httpException = ex;
            }
            catch (ControllerCreationException ex) // would be thrown by the Sitecore controller factory
            {
                httpException = WrapWithHttpException(ex);
            }

            if (controller == null)
                controller = CreateControllerUsingConfigurationFactory(controllerName)
                             ?? CreateControllerFromUnknown(requestContext, controllerName);

            if (controller == null && httpException != null)
                throw httpException;

            if (controller == null)
                throw new HttpException(404, "Unable to resolve controller");

            return controller;
        }

        protected virtual IController CreateControllerFromUnknown(RequestContext requestContext, string controllerName)
        {
            if (TypeHelper.LooksLikeTypeName(controllerName))
                return _objectFactory.GetObject(controllerName) as IController;

            var controllerType = base.GetControllerType(requestContext, controllerName);
            return _objectFactory.GetObject(controllerType) as IController;
        }

        protected virtual IController CreateControllerUsingConfigurationFactory(string controllerName)
        {
            XmlNode node;
            try
            {
                if (_knownTypes.ContainsKey(controllerName))
                    return Factory.CreateObject<IController>(_knownTypes[controllerName]);

                node = Factory.GetConfigNode(controllerName);
            }
            catch
            {
                node = null;
            }
            if (node != null)
            {
                _knownTypes.Add(controllerName, node);
                return Factory.CreateObject<IController>(node);
            }

            Log.Debug($"Couldn't find {controllerName} controller in Web.config", GetType());
            return null;
        }

        private HttpException WrapWithHttpException(ControllerCreationException ccex)
        {
            return new HttpException(500, ccex.Message, ccex);
        }
    }
}
