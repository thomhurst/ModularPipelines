using System.Text.Json;
using Pipeline.NET.Context;
using Pipeline.NET.MicrosoftTeams.Models;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.MicrosoftTeams;

public abstract class MicrosoftTeamsWebhookModule : Module<HttpResponseMessage>
{
    protected MicrosoftTeamsWebhookModule(IModuleContext context) : base(context)
    {
    }

    protected abstract MicrosoftTeamsAdaptiveCard Card { get; }
    protected abstract Uri WebhookUri { get; }

    protected override async Task<ModuleResult<HttpResponseMessage>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var serializedCard = JsonSerializer.Serialize(MicrosoftTeamsCardWrapper.Wrap(Card), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var cardsRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(serializedCard),
            RequestUri = WebhookUri
        };

        var responseMessage = await Context.Get<HttpClient>()!.SendAsync(cardsRequest, cancellationToken);

        return responseMessage.EnsureSuccessStatusCode();
    }
}