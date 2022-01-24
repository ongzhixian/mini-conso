﻿using Microsoft.Extensions.Logging;

namespace Conso.Services;

internal static class LogProgramMessage
{
    internal readonly static Action<ILogger, Exception?> ApplicationStart = LoggerMessage.Define(
        LogLevel.Information, new EventId(285802, "Application start"), string.Empty);

    internal readonly static Action<ILogger, Exception?> ApplicationEnd = LoggerMessage.Define(
        LogLevel.Information, new EventId(296395, "Application end"), string.Empty);
}
