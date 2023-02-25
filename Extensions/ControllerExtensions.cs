using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Idevs.Extensions;

public static class ControllerExtensions
{
    public static string RenderView<T>(this Controller controller, string path, string viewName, T model, bool partial = false)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            viewName = controller.ControllerContext.ActionDescriptor.ActionName;
        }

        controller.ViewData.Model = model;

        using var writer = new StringWriter();
        IViewEngine? viewEngine =
            controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

        if (viewEngine is null)
        {
            throw new Exception($"A view engine is required");
        }

        var viewResult = viewEngine.GetView(path, viewName, isMainPage: true);
        if (viewResult.Success == false)
        {
            throw new Exception($"A view with the name {viewName} could not be found");
        }

        var viewContext = new ViewContext(
            controller.ControllerContext,
            viewResult.View,
            controller.ViewData,
            controller.TempData,
            writer,
            new HtmlHelperOptions()
        );

        viewResult.View.RenderAsync(viewContext);

        return writer.GetStringBuilder().ToString();
    }
}
