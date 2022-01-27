using System.Diagnostics.CodeAnalysis;

namespace Conso.Models;

[ExcludeFromCodeCoverage]
public class ApplicationSetting
{
    public string Version { get; set; } = string.Empty;
    public string RunType { get; set; } = string.Empty;
}
