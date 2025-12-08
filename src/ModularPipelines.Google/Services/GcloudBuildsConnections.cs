using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("builds")]
public class GcloudBuildsConnections
{
    public GcloudBuildsConnections(
        GcloudBuildsConnectionsCreate create,
        GcloudBuildsConnectionsUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudBuildsConnectionsCreate Create { get; }

    public GcloudBuildsConnectionsUpdate Update { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudBuildsConnectionsAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudBuildsConnectionsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudBuildsConnectionsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudBuildsConnectionsGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudBuildsConnectionsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudBuildsConnectionsListOptions(), token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudBuildsConnectionsSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}