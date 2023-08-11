using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.MicrosoftTeams.Models;
using ModularPipelines.MicrosoftTeams.Options;

namespace ModularPipelines.MicrosoftTeams;

internal class MicrosoftTeams : IMicrosoftTeams
{
    private readonly HttpClient _httpClient;

    public MicrosoftTeams(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> PostMicrosoftTeamsCard(MicrosoftTeamsWebHookCardOptions options, CancellationToken cancellationToken = default)
    {
        var serializedCard = JsonSerializer.Serialize(MicrosoftTeamsCardWrapper.Wrap(options.Card), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var cardsRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(serializedCard),
            RequestUri = options.WebHookUri
        };

        var responseMessage = await _httpClient.SendAsync(cardsRequest, cancellationToken);

        return responseMessage.EnsureSuccessStatusCode();
    }
}
