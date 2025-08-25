# Changelog

## 0.2.3 (2025-08-26)

### Improved

- **ChromeHelper Apple Silicon Detection**: Enhanced Apple Silicon (ARM64) detection with more reliable fallback mechanisms
    - Added native system call detection using `sysctl hw.optional.arm64`
    - Improved fallback detection using `RuntimeInformation.OSArchitecture` and environment variables
    - Better compatibility across different macOS versions and execution contexts

## 0.2.2 (2025-08-26)

### Added

- **ChromeHelper ARM64 Support**: Added support for ARM64 architecture on macOS (Apple Silicon)
- **ChromeHelper Linux Support**: Added support for Linux operating system with both x64 and ARM64 architectures
- Enhanced architecture detection using `RuntimeInformation.ProcessArchitecture` for proper Chrome binary selection

### Changed

- **ChromeHelper.GetChromePath()**: Updated to detect system architecture and select appropriate Chrome binary paths:
  - macOS: `chrome-mac-arm64` for Apple Silicon, `chrome-mac-x64` for Intel
  - Linux: `chrome-linux-arm64` for ARM64, `chrome-linux64` for x64
  - Windows: Continues to use `chrome-win64` (compatible with ARM64 through emulation)

## 0.2.1 (2025-08-20)

## 0.2.0 (2025-08-16)

### Breaking Changes

- **Autofac Integration**: Introduced Autofac as the primary dependency injection container
- **ServiceExtensions Modernization**: Replaced traditional ServiceExtensions with Autofac modules
- **Enhanced Registration**: Improved service registration with better lifetime management

### Added

- `IdevsModule`: New Autofac module for automatic service registration
- `UseIdevsAutofac()`: Extension method for WebApplicationBuilder to configure Autofac
- `StaticServiceLocator`: New thread-safe static service locator with Autofac support
- `UseIdevsStaticServiceLocator()`: Extension method for WebApplication to initialize static service resolution
- **Enhanced Registration Attributes**: New standard attributes (`[Scoped]`, `[Singleton]`, `[Transient]`) with advanced features
- **Named Service Registration**: Support for service keys in Autofac (e.g., `[Scoped(ServiceKey = "mykey")]`)
- **Explicit Service Types**: Ability to specify exact service interfaces (`[Scoped(ServiceType = typeof(IMyService))]`)
- **Self-Registration**: Option to register services without interfaces (`[Scoped(AllowSelfRegistration = true)]`)
- Multiple Autofac configuration overloads for advanced scenarios
- Support for custom container configuration
- Better performance through Autofac's optimized dependency resolution
- Static service resolution with scoping support
- Automatic container type detection (Autofac vs traditional DI)

### Changed

- **Service Registration**: `AddIdevsCorelibServices()` now supports both Autofac and fallback scenarios
- **Lifetime Management**: Improved service lifetime scoping with Autofac
- **Module-Based Architecture**: Services organized into logical modules
- **Backward Compatibility**: Legacy ServiceExtensions still supported

### Deprecated

- `RegisterServices()`: Marked as obsolete, functionality merged into `AddIdevsCorelibServices()`

### Migration Guide

**Recommended (New Autofac approach):**
```csharp
// Replace this
builder.Services.AddIdevsCorelibServices();

// With this
builder.UseIdevsAutofac();
```

**Legacy Support:**
```csharp
// This still works for backward compatibility
builder.Services.AddIdevsCorelibServices();
```

## 0.1.1 (2025-07-21)

### Refactor

- Update to use Serene 8.8.6
- Refactor all

## 0.1.0 (2024-12-05)

### Breaking Changes

- Support only dotnet 8 (if you want to use dotnet 6, please use version 0.0.92)
- Start using Serene 8.8.1
- Refactor project structure, namespace, and class name
- Remove unnecessary code

## 0.0.92 (2024-09-03)

### Add

- Add ContentResponse to support string return response

## 0.0.91 (2024-08-29)

### Changes

- Change CreatePdfViewResult to use FileStreamResult instead of FileContentResult

## 0.0.90 (2024-08-29)

### Add

- Add CreatePdfViewResult to PdfExporter for direct open in browser instead of download first

## 0.0.89 (2024-08-28)

### Update

- Add --no-sandbox and --disable-extensions to launchoption on puppeteer

## 0.0.88 (2024-08-26)

### Update

- Update libraries

## 0.0.87 (2024-08-17)

### Update

- Add browserPath to PdfExporter.Export to use existing Chrome installed instead of download everytime.

## 0.0.86 (2024-08-17)

### Fixed

- Fixed RegisterService throw System.Reflection.ReflectionTypeLoadException with 'SqlGuidCaster'

## 0.0.85 (2024-08-17)

### Updates

- Update library version
- Update RepositoryBase, move default dialect instance on DotNet 8

## 0.0.84 (2024-03-24)

### Updates

- Update library version
- Update RepositoryBase to share ServiceProvider on DotNet 8

## 0.0.83 (2024-01-27)

### Add

- Add EnumEditorAttribute that support EnumKey

### Updates

- Update Serenity package to version 8.2.2 for DotNet 8

## 0.0.82 (2024-01-23)

### Changes

- Add different ExceptionLog for .Net6 and .Net8

## 0.0.81 (2024-01-23)

### Try

- Remove get required service for ExceptionLog for DotNet 8

## 0.0.80 (2024-01-15)

### Fixed

- Fixed RepositoryBase for DotNet 8

## 0.0.79 (2024-01-14)

### Changes

- Rollback to original with some refactor. Error from Microsoft.Data.SqlClient have to install new version

## 0.0.78 (2024-01-14)

### Changes

- Refactory RegisterServices again

## 0.0.77 (2024-01-14)

### Changes

- Refactor RegisterServices

## 0.0.76 (2024-01-14)

### Changes

- Remove IConfiguration from RegisterServices
- Rename AddIdevCoreLibServices and also call RegisterServices

## 0.0.75 (2024-01-13)

### Updates

- Add service register

## 0.0.74 (2024-01-13)

### Changes

- Add support DotNet 8.0 with serenity 8.1.5
- Remove unnecessary part
- Remove support aggregate columns from ExcelExporter it's cause performance issue

## 0.0.73 (2023-10-23)

### Updates

- Update CheckboxButtonEditorAttribute add option IsStringId

## 0.0.72 (2023-10-15)

### Changes

- Rollback serenity to version 6.5.1
- Rollback ILogger to IExceptionLogger may be implement after test
- Update RepositoryBase
- Update ExcelExporter
- Upgrade library

## 0.0.71 (2023-10-15)

### Changes

- Remove PugPDF.Core
- Update IdevsExportRequest, PageMargin
- Add PageSize with PageSizes enum and PageOrientations enum

## 0.0.70 (2023-10-14)

### Updates

- Add method GetPageSize and GetMargin to IdevsExportRequest

## 0.0.69 (2023-10-14)

### Updates

- Update IdevsExportRequest by add property PageSize and PageMargin

## 0.0.68 (2023-10-14)

### Updates

- Add PuppeteerSharp library for export to pdf and will be remove PugPDF.Core and WkHtmlToPdf later.

## 0.0.67 (2023-08-27)

### Updates

- Add default font for ClosedXML

## 0.0.66 (2023-06-10)

### Updates

- Updates mapping display format to cell format in Excel Exporter

## 0.0.65 (2023-06-10)

### Updates

- Add space before %

## 0.0.64 (2023-06-10)

### Added

- Add support display percent for Excel Export
- Add DisplayPercentageAttribute

## 0.0.63 (2023-06-08)

### Fixed

- Fixed excel export with group that not group on the first column

## 0.0.62 (2023-06-04)

### Updates

- Remove blank header line from report headers when generate excel

## 0.0.61 (2023-06-03)

### Added

- Add conditionRange to IdevsExportRequest

## 0.0.60 (2023-06-01)

### Fixed

- Rename and remove TableTheme property

## 0.0.59 (2023-06-01)

## Changes

- Changes ExcelExporter Export signature

## 0.0.58 (2023-06-01)

### Fixed

- Fixed nullable type for theme style

## 0.0.57 (2023-06-01)

### Added

- Add support customize table theme style for ExcelExporter

## 0.0.56 (2023-05-31)

### Fixed

- Finally fixed grouping for ExcelExporter

## 0.0.55 (2023-05-31)

### Fixed

- Fixed grouping for ExcelExporter again #2

## 0.0.54 (2023-05-31)

### Fixed

- Fixed grouping for ExcelExporter again

## 0.0.53 (2023-05-31)

### Fixed

- Fixed mistake for grouping on ExcelExporter

## 0.0.52 (2023-05-31)

### Fixed

- Fixed grouping for ExcelExporter

## 0.0.51 (2023-05-31)

### Fixed

- Fixed row calculation for Group on ExcelExporter

## 0.0.50 (2023-05-31)

### Added

- Add feature for ExcelExporter to support group in aggregate columns

## 0.0.49 (2023-05-27)

### Fixed

- Fixed summary row title again

## 0.0.48 (2023-05-27)

### Fixed

- Fixed summary row title

## 0.0.47 (2023-05-27)

### Updates

- Added entity property to IdevsExportRequest

## 0.0.46 (2023-05-27)

### Updates

- Add support label for aggregate columns
- Fixed boundary range table

## 0.0.45 (2023-05-26)

### Changes

- Changes to use total rows approach fo aggregate columns

## 0.0.44 (2023-05-26)

### Fixed

- Fixed error result for aggregate columns

## 0.0.43 (2023-05-26)

### Added

- Add enum type for Aggregate

## 0.0.42 (2023-05-26)

### Updates

- Prevent ambiguous call for export with aggregate column

## 0.0.41 (2023-05-26)

### Updates

- Add support aggregate column for summary row
- Remove unnecessary property for ExcelExportRequest

## 0.0.40 (2023-05-24)

### Updates

- Add property to IdevsExportRequest

## 0.0.39 (2023-04-16)

### Updates

- Move excel header to after AdjustToContents

## 0.0.38 (2023-04-15)

### Added

- Add EnumExtensions.GetDescription

## 0.0.37 (2023-04-14)

### Updates

- Update export excel report arguments

## 0.0.36 (2023-04-14)

### Added

- Added export excel report using CloxedXML.Report

## 0.0.35 (2023-04-14)

### Fixed

- Final fix error Excel export with number formatting

## 0.0.34 (2023-04-12)

### Fixed

- Fixed error Excel export with format again

## 0.0.33 (2023-04-12)

### Fixed

- Fixed error Excel export with format

## 0.0.32 (2023-04-12)

### Fixed

- Fixed error with register service using reflection

## 0.0.31 (2023-04-12)

### Added

- Add register service using reflection

## 0.0.30 (2023-04-12)

### Updated

- Update ExcelExporter to display correct format

## 0.0.29 (2023-04-03)

### Added

- Add ResponseModel

## 0.0.28 (2023-04-02)

### Added

- Add DateMonthFormatterAttribute

## 0.0.27 (2023-04-02)

### Added

- Added DateMonthEditorAttribute

## 0.0.26 (2023-03-26)

### Fixed

- Fixed error from TrimModel

## 0.0.25 (2023-03-26)

### Added

- ModelExtensions TrimModel

## 0.0.24 (2023-03-24)

### Fixed

- Fixed starting row

## 0.0.23 (2023-03-24)

### Added

- Added ExcelExport parameter to add headers

## 0.0.22 (2023-03-24)

### Changes

- change ExcelExport function Generate from private to public to allow changes column title

## 0.0.21 (2023-03-23)

### Changes

- Add more property to IdevsExportRequest

## 0.0.20 (2023-03-22)

### Changes

- Add interface IIdevsExcelExporter inherited from IExcelExporter
- Rename ExcelExporter to IdevsExcelExporter
- Rename PdfExporter to IdevsPdfExporter

## 0.0.19 (2023-03-22)

### Updated

- IdevsExportRequest added Filters property

## 0.0.18 (2023-03-13)

### Added

- LookupFormatterAttribute

## 0.0.17 (2023-03-09)

### Added

- CheckboxButtonEditorAttribute

## 0.0.16 (2023-03-05)

## Fixed

Fixed targets to install dependencies packages

## 0.0.15 (2023-03-04)

## Fixed

Fixed targets for install npm packages idevs.corlib

## 0.0.14 (2023-03-04)

### Changed

Revert to previous targets and let user install dependencies himself

## 0.0.13 (2023-03-04)

### Changed

Update targets to install needed dependencie packages

## 0.0.12 (2023-03-04)

### Added

- ComponentModes/DisplayNumberFormatAttribute

### Changed

Rename folder ComponentModel to ComponentModels

## 0.0.11 (2023-03-03)

### Fixed

Fixed project files to copy scripts

## 0.0.10 (2023-03-03)

### Changes

All new changes

## 0.0.9 (2023-03-01)

### Added

- Repositories/RepositoryBase
- Helpers/ViewRenderer
- Models/PdfContentResult
- Services/ExcelExporter
- Services/PdfExporter
- Services/StaticServiceProvider

### Changes

- Extensions/TextLocalizerExtensions -> change name and add more methods

### Fixed

Fixed ZeroDisplayFormatterAttribute and CheckboxFormatterAttribute

## 0.0.8 (2023-02-28)

### Fixed

Try to fixed display text not show on grid

## 0.0.7 (2023-02-28)

### Changed

Update arguments assignments for formatter attributes

## 0.0.6 (2023-02-28)

### Changed

Try to change arguments assignment

## 0.0.5 (2023-02-28)

## Fixed

- Fixed ZeroDisplayFormatterAttribute's arguments
- Fixed CheckboxFormatterAttribute's attributes

## 0.0.4 (2023-02-28)

## Changed

- Update ZeroDisplayFormatterAttribute options
- Update CheckboxFormatterAttribute options

## 0.0.3 (2023-02-28)

## Added

- ZeroDisplayFormatterAttribute

## Changed

- Removed ZeroToBlankFormatterAttribute

## 0.0.2 (2023-02-26)

### Changed

- Removed Content/css from nuget package and use from npm package @idevs/corelib instead

## 0.0.1 (2023-02-26)

First published on nuget.org.

### Added

- ComponentModel

    - DisplayDateFormatAttribute with default format dd/MM/yyyy
    - DisplayDateTimeFormatAttribute with default format dd/MM/yyyy HH:mm:ss or dd/MM/yyyy HH:mm on your choice
    - DisplayTimeFormatAttribute with default format HH:mm:ss or HH:mm:ss on your choice
    - ColumnWidthAttribute extends FormWidthAttribute to support bootstrap's col-xxl
    - FullColumnWidthAttribute inherits from ColumnWidthAttribute
    - HalfColumnWidthAttribute inherits from ColumnWidthAttribute
    - CheckboxFormatterAttribute
    - ZeroToBlankFormatterAttribute

- Extensions

    - ControllerExtensions
    - EntityQueryExtensions
    - NumberExtensions
    - TextLocalizerExtensions

- Content/css
    - idevs.dropdown.css
    - idevs.font.css
    - idevs.print.css
