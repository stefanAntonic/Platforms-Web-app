﻿using System.Text;
using System.Text.Json;
using PlatformService.Dto;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient  httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json"
        );
        Console.WriteLine($"{_configuration["CommandService"]}");
        var response = await  _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "--> Sync POST to CommandService was OK!"
            : "--> Sync POST to CommandService was NOT OK!");
    }
}