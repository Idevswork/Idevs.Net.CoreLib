
# Idevs.Net.CoreLib

A library to extended Serenity Framework.





## Installation

To install this module, run the following command in your computer terminal:

```console
  dotnet add package Idevs.Net.CoreLib
```

Add the following code to /Initialization/Startup.cs

```console
  public void ConfigureServices(IServiceCollection services)
  {
    --- ommited ---

    services.AddScoped<IViewPageRenderer, ViewPageRenderer>();
    services.AddScoped<IPdfExporter, PdfExporter>();
    services.AddScoped<IExcelExporter, ExcelExporter>();
  }

  --- ommited ---
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    StaticServiceProvider.Provider = app.ApplicationServices;

    --- ommited ---
  }
```

## License

[MIT](https://choosealicense.com/licenses/mit/)


## Authors

- [@klomkling](https://www.github.com/klomkling)


### Usage
