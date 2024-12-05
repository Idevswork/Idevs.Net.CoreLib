using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Idevs;

public interface IViewPageRenderer
{
    Task<string> RenderViewAsync<TModel>(string viewName, TModel model);
}

public class ViewPageRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider) : IViewPageRenderer
{
    public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model)
    {
        var actionContext = GetActionContext();
        var view = FindView(actionContext, viewName);

        await using var output = new StringWriter();
        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary<TModel>(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: new ModelStateDictionary())
            {
                Model = model
            },
            new TempDataDictionary(
                actionContext.HttpContext,
                tempDataProvider),
            output,
            new HtmlHelperOptions()
        );

        await view.RenderAsync(viewContext);
        return output.ToString();
    }

    private IView FindView(ActionContext actionContext, string viewName)
    {
        var getViewResult = viewEngine.GetView(null, viewName, false);
        if (getViewResult.Success)
        {
            return getViewResult.View;
        }

        var findViewResult = viewEngine.FindView(actionContext, viewName, false);
        if (findViewResult.Success)
        {
            return findViewResult.View;
        }

        var searchLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
        var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(
                searchLocations));
        throw new InvalidOperationException(errorMessage);
    }

    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}