using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Conso.Services;

internal static class EventIdFactory
{
    internal static EventId EventId(string text)
    {
        byte[] hashBytes = SHA1.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));

        var result = 0;

        for (var i = 0; i < hashBytes.Length; i++) {
            result += (hashBytes[i] * (i + 1));
        }

        return new EventId(result, text);
    }
}
