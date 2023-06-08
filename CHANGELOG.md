# Changelog

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
