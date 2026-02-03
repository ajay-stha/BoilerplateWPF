using Microsoft.Extensions.Logging;
using Moq;

namespace App.UnitTests.Extensions;

public static class LoggerMoqExtensions
{
    /// <summary>
    /// Verifies that a specific LogLevel was used.
    /// </summary>
    public static void VerifyLogLevel<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Func<Times> times)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((_, _) => true),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }

    /// <summary>
    /// Verifies that a log message containing a specific fragment was logged at the given LogLevel.
    /// </summary>
    public static void VerifyLogMessageContains<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string fragment, Func<Times> times)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains(fragment)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }
}
