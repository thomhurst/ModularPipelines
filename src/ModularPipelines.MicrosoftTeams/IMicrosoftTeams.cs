namespace ModularPipelines.MicrosoftTeams;

public interface IMicrosoftTeams
{
    Task<HttpResponseMessage> PostCardAsync(MicrosoftTeamsWebHookCardOptions options, CancellationToken cancellationToken = default);
}