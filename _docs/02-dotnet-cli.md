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


## Templates

In the project root, create a `.template.config` directory.
In `.template.config` directory, add a file call `template.json` with the following content:

```json
{
    "$schema": "http://json.schemastore.org/template",
    "author": "ONG Zhi Xian",
    "classifications": [ "Common", "Console", "C#9" ],
    "identity": "MiniConso.Conso",
    "name": "Mini console project",
    "shortName": "miniconsole",
    "tags": {
        "language": "C#",
        "type": "project"
    }
}
```

At project root run the following command:

`dotnet new --install .\`

`dotnet new --uninstall .\`


Json Template Wiki
https://github.com/dotnet/templating/wiki

Json Template Schema
http://json.schemastore.org/template


## 

```ps1
$bytes = [Text.Encoding]::UTF8.GetBytes("mytext")
$b = [System.Security.Cryptography.SHA1CryptoServiceProvider]::new().ComputeHash([Text.Encoding]::UTF8.GetBytes("MiniTools.Web.Controllers.LoginController"))
$ans = 0
for ($i = 0; $i -lt $b.Length; $i++) {
    # Write-Host $($b[$i] * ($i + 1))
    $ans = $ans + $($b[$i] * ($i + 1))
}

"$ans$($ans % 11)" | Set-Clipboard
Write-Host "Final: $ans$($ans % 11)" 
```

```ps1: one-line equivalent
$w,$s=0; [System.Security.Cryptography.SHA1CryptoServiceProvider]::new().ComputeHash([Text.Encoding]::UTF8.GetBytes("MiniTools.Web.Controllers.LoginController")).ForEach({$_*++$w}).ForEach({$s+=$_});"$s$($s%11)"
```

```ps1
function calcId($text) {
    $w,$s=0; [System.Security.Cryptography.SHA1CryptoServiceProvider]::new().ComputeHash([Text.Encoding]::UTF8.GetBytes($text)).ForEach({$_*++$w}).ForEach({$s+=$_});"$s$($s%11)" 
}
```

Test
"MiniTools.Web.Controllers.LoginController" should give = 251394

