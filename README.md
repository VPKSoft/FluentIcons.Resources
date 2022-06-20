# FluentIcons.Resources
SVG icons from the Microsoft Fluent UI System Icons as a resource package.

This is a machine-generated package of the [Microsoft Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons). The SVG files are packaged into resources and released as a NuGet package which makes this suitable for desktop application use. All the icons are binary `byte[]` resources so a conversion to a displayable icon is required unless SVG is directly supported by the platform.

The SVG icons in the package are divided in different sizes and not all size sets are complete. However with SVG the size doesn't matter, just scale the icon you wish to use to any desired size.

The icon resources are generated using the [CS-Script](https://github.com/oleg-shilo/cs-script), to install the CS-Script:
* On Windows, [Chocolatey](https://chocolatey.org), `choco install cs-script`
* On Linux, DEB, `repo=https://github.com/oleg-shilo/cs-script/releases/download/v4.4.7.0/; file=cs-script_4.4-7.deb; wget $repo$file; sudo dpkg -i $file; rm $file`

### Few SVG libraries for example
* [SVG](https://github.com/svg-net/SVG)
* [Svg.Skia](https://github.com/wieslawsoltes/Svg.Skia)

### Build your self
* Checkout this repository
* Checkout the [Microsoft Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons)
* Use the following directory structure:
```
- ParentDir
     |
      - FluentIcons.Resources
      - fluentui-system-icons
```
* Change to directory: `FluentIcons.Resources/GenerateResources`
* Run: `css ./resource_generate.cs`
* Rebuild the project

### Package resource structure
```
FluentIcons
     .Resources
          .Filled
               .Filled16
               .Filled20               
               .Filled24               
               .Filled28               
               .Filled32               
               .Filled48               
          .Regular
               .Regular16
               .Regular20               
               .Regular24               
               .Regular28               
               .Regular32               
               .Regular48                              
```

### Thanks to
* [Microsoft Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons) (obvious)
* [CS-Script](https://github.com/oleg-shilo/cs-script)
