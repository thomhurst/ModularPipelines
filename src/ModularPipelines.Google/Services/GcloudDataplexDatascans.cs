using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex")]
public class GcloudDataplexDatascans
{
    public GcloudDataplexDatascans(
        GcloudDataplexDatascansCreate create,
        GcloudDataplexDatascansJobs jobs,
        GcloudDataplexDatascansUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Jobs = jobs;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDataplexDatascansCreate Create { get; }

    public GcloudDataplexDatascansJobs Jobs { get; }

    public GcloudDataplexDatascansUpdate Update { get; }

    public async Task<CommandResult> Delete(GcloudDataplexDatascansDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDataplexDatascansDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudDataplexDatascansGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDataplexDatascansListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDataplexDatascansListOptions(), token);
    }

    public async Task<CommandResult> Run(GcloudDataplexDatascansRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudDataplexDatascansSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}