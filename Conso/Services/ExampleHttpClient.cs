﻿using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public interface IExampleHttpClient
{
    Task<string> GetWeatherForcastAsync();
}

public class ExampleHttpClient : IExampleHttpClient
{
    private readonly ILogger<ExampleHttpClient> logger;
    private readonly HttpClient httpClient;

    public ExampleHttpClient(ILogger<ExampleHttpClient> logger, HttpClient httpClient, IOptionsMonitor<HttpClientSetting> optionsMonitor)
    {
        HttpClientSetting setting = optionsMonitor.Get("HttpClients:ExampleWeatherForecast");

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpClient = httpClient;
        httpClient.BaseAddress = new Uri(setting.BaseAddress);
    }

    public async Task<string> GetWeatherForcastAsync()
    {
        var responseMessage = await httpClient.GetAsync("/WeatherForecast");

        responseMessage.EnsureSuccessStatusCode();

        var content = await responseMessage.Content.ReadAsStringAsync();

        logger.LogInformation("Forecast content is {content}", content);
        return content;
    }
}