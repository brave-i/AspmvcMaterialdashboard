using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;

namespace Chatison.Helpers
{
    public static class Extensions
    {
        public static void BindInRequestScope<T1>(this IUnityContainer container)
        {
            container.RegisterType<T1>(new HierarchicalLifetimeManager());
        }

        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1>(this IUnityContainer container)
        {
            container.RegisterType<T1>(new ContainerControlledLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }

        public static void SetSuccessResponse(this Controller controller, string content, bool nextPage = false)
        {
            if (!nextPage)
            {
                controller.ViewBag.Response = $"success|{content}";
            }
            else
            {
                controller.TempData["Response"] = $"success|{content}";
            }
        }

        public static void SetErrorResponse(this Controller controller, string content, bool nextPage = false)
        {
            if (!nextPage)
            {
                controller.ViewBag.Response = $"error|{content}";
            }
            else
            {
                controller.TempData["Response"] = $"error|{content}";
            }
        }

        public static string GetBaseUrl(this HttpRequestBase request)
        {
            if (request?.Url == null)
            {
                return string.Empty;
            }

            return request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host +
                   (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port) + "/";
        }

        public static string GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        public static IEnumerable<string> GetErrorList(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage.Replace("'", "")));
        }

        public static IEnumerable<string> ToErrorList(this ModelStateDictionary modelState)
        {
            if (modelState == null
                || modelState.IsValid)
            {
                return null;
            }

            return modelState
                .Values
                .Where(x => x.Errors.Count != 0)
                .Select(x => x.Errors.First().ErrorMessage);
        }
    }
}