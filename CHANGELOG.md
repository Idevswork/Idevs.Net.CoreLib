# Changelog

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
