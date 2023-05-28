using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.MicrosoftTeams.Models;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.MicrosoftTeams;

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