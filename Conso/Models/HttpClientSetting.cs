using System.Diagnostics.CodeAnalysis;

namespace Conso.Models;

[ExcludeFromCodeCoverage]
public class HttpClientSetting
{
    public string BaseAddress { get; set; } = string.Empty;
}
