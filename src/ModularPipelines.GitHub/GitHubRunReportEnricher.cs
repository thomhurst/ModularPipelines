using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.GitHub;

internal sealed class GitHubRunReportEnricher(IServiceScopeFactory serviceScopeFactory)
    : IRunReportEnricher
{
    public ValueTask EnrichAsync(
        RunReportEnrichmentContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var scope = serviceScopeFactory.CreateScope();
        var environment = scope.ServiceProvider.GetRequiredService<IGitHubEnvironmentVariables>();

        context.GitSha = NullIfEmpty(environment.Sha) ?? context.GitSha;
        context.GitBranch = NullIfEmpty(environment.HeadRef)
            ?? NullIfEmpty(environment.RefName)
            ?? context.GitBranch;
        context.CiRunUrl = CreateRunUrl(environment) ?? context.CiRunUrl;
        return ValueTask.CompletedTask;
    }

    private static string? CreateRunUrl(IGitHubEnvironmentVariables environment)
    {
        var serverUrl = NullIfEmpty(environment.ServerUrl)?.TrimEnd('/');
        var repository = NullIfEmpty(environment.Repository)?.Trim('/');
        var runId = NullIfEmpty(environment.RunId);
        return serverUrl is null || repository is null || runId is null
            ? null
            : $"{serverUrl}/{repository}/actions/runs/{runId}";
    }

    private static string? NullIfEmpty(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}
