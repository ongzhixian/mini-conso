using System.Diagnostics.CodeAnalysis;

namespace Conso.Models;

[ExcludeFromCodeCoverage]
public class HttpClientSetting
{
    public string BaseAddress { get; set; } = string.Empty;

    public void EnsureIsValid()
    {
        if (string.IsNullOrEmpty(BaseAddress)) {
            throw new InvalidDataException("Invalid BaseAddress");
        }
    }
}
