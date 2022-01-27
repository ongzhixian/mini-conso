using Conso.Models;
using Conso.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Conso.Tests.Services;

[TestClass]
public class ExampleServiceTests
{
    private Mock<IExampleHttpClient> mockHttpClient = new();
    private Mock<ILogger<ExampleService>> mockLogger = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        mockLogger = new Mock<ILogger<ExampleService>>();
        mockHttpClient = new Mock<IExampleHttpClient>();
    }


    [TestMethod()]
    public void ExampleServiceTest()
    {
        ExampleService service = new(mockLogger.Object, mockHttpClient.Object);

        Assert.IsNotNull(service);
    }

    [ExcludeFromCodeCoverage]
    [TestMethod()]
    public void ExampleServiceNullLoggerTest()
    {
        ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
            new ExampleService(null, mockHttpClient.Object)
        );

        Assert.IsNotNull(ex);
        Assert.AreEqual<string>("Value cannot be null. (Parameter 'logger')", ex.Message);
    }

    [ExcludeFromCodeCoverage]
    [TestMethod()]
    public void ExampleServiceNullClientTest()
    {
        ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
            new ExampleService(mockLogger.Object, null)
        );

        Assert.IsNotNull(ex);
        Assert.AreEqual<string>("Value cannot be null. (Parameter 'client')", ex.Message);
    }

    [TestMethod]
    public async Task TestMethod1Async()
    {
        mockHttpClient.Setup(m => m.GetWeatherForcastAsync()).ReturnsAsync("somestring");

        await new ExampleService(mockLogger.Object, mockHttpClient.Object).DoWorkAsync();

        mockHttpClient.Verify(x => x.GetWeatherForcastAsync(), Times.Once);
    }
}