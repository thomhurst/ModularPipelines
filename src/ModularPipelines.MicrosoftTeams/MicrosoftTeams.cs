using System.Text.Json;
using ModularPipelines.Http;
using ModularPipelines.MicrosoftTeams.Models;
using ModularPipelines.MicrosoftTeams.Options;

namespace ModularPipelines.MicrosoftTeams;

internal class MicrosoftTeams : IMicrosoftTeams
{
    private readonly IHttp _http;

    public MicrosoftTeams(IHttp http)
    {
        _http = http;
    }

    public async Task<HttpResponseMessage> PostMicrosoftTeamsCard(MicrosoftTeamsWebHookCardOptions options, CancellationToken cancellationToken = default)
    {
        var serializedCard = JsonSerializer.Serialize(MicrosoftTeamsCardWrapper.Wrap(options.Card), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var cardsRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(serializedCard),
            RequestUri = options.WebHookUri,
        };

        return await _http.SendAsync(cardsRequest, cancellationToken);
    }
}