using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Conso.Services.Tests;

[TestClass()]
public class AuthenticationServiceTests
{
    private Mock<ILogger<AuthenticationService>> mockLogger = new();
    private Mock<IAuthenticationHttpClient> mockClient = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        mockLogger = new Mock<ILogger<AuthenticationService>>();
        mockClient = new Mock<IAuthenticationHttpClient>();
    }

    [TestMethod()]
    public void AuthenticationServiceTest()
    {
        AuthenticationService authentication = new(mockLogger.Object, mockClient.Object);

        Assert.IsNotNull(authentication);
    }

    [ExcludeFromCodeCoverage]
    [TestMethod()]
    public void AuthenticationServiceNullLoggerTest()
    {
        ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
            new AuthenticationService(null, mockClient.Object));

        Assert.IsNotNull(ex);
        Assert.AreEqual<string>("Value cannot be null. (Parameter 'logger')", ex.Message);
    }

    [ExcludeFromCodeCoverage]
    [TestMethod()]
    public void AuthenticationServiceNullClientTest()
    {
        ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
            new AuthenticationService(mockLogger.Object, null));

        Assert.IsNotNull(ex);
        Assert.AreEqual<string>("Value cannot be null. (Parameter 'client')", ex.Message);
    }

    [TestMethod()]
    public void DoWorkAsyncTest()
    {
        AuthenticationService authentication = new(mockLogger.Object, mockClient.Object);

        System.Threading.Tasks.Task? result = authentication.DoWorkAsync();

        Assert.IsNotNull(result);
    }
}
