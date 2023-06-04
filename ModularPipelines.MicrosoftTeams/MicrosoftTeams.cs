using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.MicrosoftTeams.Models;
using ModularPipelines.MicrosoftTeams.Options;

namespace ModularPipelines.MicrosoftTeams;

public class MicrosoftTeams<T> : IMicrosoftTeams<T>
{
    public IModuleContext<T> Context { get; }

    public MicrosoftTeams(IModuleContext<T> context)
    {
        Context = context;
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

        var responseMessage = await Context.Get<HttpClient>()!.SendAsync(cardsRequest, cancellationToken);

        return responseMessage.EnsureSuccessStatusCode();
    }
}