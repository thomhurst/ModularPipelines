using ModularPipelines.MicrosoftTeams.Options;

namespace ModularPipelines.MicrosoftTeams;

public interface IMicrosoftTeams
{
    Task<HttpResponseMessage> PostMicrosoftTeamsCard(MicrosoftTeamsWebHookCardOptions options, CancellationToken cancellationToken = default);
}