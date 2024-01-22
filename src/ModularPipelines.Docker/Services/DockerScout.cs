using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerScout
{
    public DockerScout(
        DockerScoutIntegration scoutIntegration,
        DockerScoutRepo scoutRepo,
        DockerScoutCache scoutCache,
        ICommand internalCommand
    )
    {
        ScoutIntegration = scoutIntegration;
        ScoutRepo = scoutRepo;
        ScoutCache = scoutCache;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public DockerScoutIntegration ScoutIntegration { get; }

    public DockerScoutRepo ScoutRepo { get; }

    public DockerScoutCache ScoutCache { get; }

    public async Task<CommandResult> Cache(DockerScoutCacheOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutCacheOptions(), token);
    }

    public async Task<CommandResult> Compare(DockerScoutCompareOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutCompareOptions(), token);
    }

    public async Task<CommandResult> Config(DockerScoutConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutConfigOptions(), token);
    }

    public async Task<CommandResult> Cves(DockerScoutCvesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutCvesOptions(), token);
    }

    public async Task<CommandResult> Enroll(DockerScoutEnrollOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutEnrollOptions(), token);
    }

    public async Task<CommandResult> Environment(DockerScoutEnvironmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutEnvironmentOptions(), token);
    }

    public async Task<CommandResult> Integration(DockerScoutIntegrationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutIntegrationOptions(), token);
    }

    public async Task<CommandResult> Policy(DockerScoutPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutPolicyOptions(), token);
    }

    public async Task<CommandResult> Quickview(DockerScoutQuickviewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutQuickviewOptions(), token);
    }

    public async Task<CommandResult> Recommendations(DockerScoutRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutRecommendationsOptions(), token);
    }

    public async Task<CommandResult> Repo(DockerScoutRepoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutRepoOptions(), token);
    }

    public async Task<CommandResult> Sbom(DockerScoutSbomOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutSbomOptions(), token);
    }

    public async Task<CommandResult> Stream(DockerScoutStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutStreamOptions(), token);
    }

    public async Task<CommandResult> Version(DockerScoutVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutVersionOptions(), token);
    }

    public async Task<CommandResult> Watch(DockerScoutWatchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutWatchOptions(), token);
    }
}
