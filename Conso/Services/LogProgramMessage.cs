using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Conso.Services;

[ExcludeFromCodeCoverage]
internal static class LogProgramMessage
{
    // Conso.Services.LogProgramMessage:Application start
    internal readonly static Action<ILogger, Exception?> ApplicationStart = LoggerMessage.Define(
        LogLevel.Information, new EventId(266387, "Application start"), "Application start");

    // Conso.Services.LogProgramMessage:Application end
    internal readonly static Action<ILogger, Exception?> ApplicationEnd = LoggerMessage.Define(
        LogLevel.Information, new EventId(199804, "Application end"), "Application end");
}
