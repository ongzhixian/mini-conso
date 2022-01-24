# dotnet CLI

## Install packages

dotnet add .\Conso\ package Microsoft.Extensions.Hosting
dotnet add .\Conso\ package Microsoft.Extensions.Configuration.UserSecrets
dotnet add .\Conso\ package Microsoft.Extensions.Http


## Add secrets

dotnet user-secrets init --project .\Conso\

dotnet user-secrets --project .\Conso\ set sample "sample secret"

## jb

Solution-wide
jb inspectcode --build --output=ignore/inspectcode-result.html --format=Html .\MiniConso.sln

Specific project
jb inspectcode --build --output=ignore/inspectcode-result.html --format=Html .\Conso\Conso.csproj

The Xml output is more informative:
jb inspectcode --build --output=ignore/inspectcode-result.xml --format=xml .\MiniConso.sln

Solution-wide
dotnet dotcover test --dcAttributeFilters=System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute --dcFilters="-:type=AspNetCoreGeneratedDocument.*;-:type=Program" --dcReportType=HTML --dcOutput=ignore/dotcover.html .\MiniConso.sln


Specific project
dotnet dotcover test --dcAttributeFilters=System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute --dcFilters="-:type=AspNetCoreGeneratedDocument.*;-:type=Program" --dcReportType=HTML --dcOutput=ignore/dotcover.html .\Conso.Tests\



# Configuring InspectCode

Add a `.editorconfig` file at the root of your project.

At the bottom of the file add the property you want to control in the following format:

`[inspection_editorconfig_property]=[error | warning | suggestion | hint | none]`

For example, "InconsistentNaming" is output as follows in the XML output:

```xml
<IssueType Id="InconsistentNaming" Category="Constraints Violations" CategoryId="ConstraintViolation" Description="Inconsistent Naming" Severity="WARNING" WikiUrl="https://www.jetbrains.com/resharperplatform/help?Keyword=InconsistentNaming" />
```

So what we have to do is find the corresponding Resharper property name (see link below) 
and add it to the `.editorconfig` file like:

```ini
# Resharper
resharper_inconsistent_naming_highlighting = none
```

Note: `jb inspectcode` command-line treats hints as none.

For a list of applicable code inspections see:
https://www.jetbrains.com/help/resharper/Reference__Code_Inspections_CSHARP.html