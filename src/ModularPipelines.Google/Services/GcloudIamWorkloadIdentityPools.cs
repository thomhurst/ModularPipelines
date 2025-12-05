using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("iam")]
public class GcloudIamWorkloadIdentityPools
{
    public GcloudIamWorkloadIdentityPools(
        GcloudIamWorkloadIdentityPoolsOperations operations,
        GcloudIamWorkloadIdentityPoolsProviders providers,
        ICommand internalCommand
    )
    {
        Operations = operations;
        Providers = providers;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamWorkloadIdentityPoolsOperations Operations { get; }

    public GcloudIamWorkloadIdentityPoolsProviders Providers { get; }

    public async Task<CommandResult> Create(GcloudIamWorkloadIdentityPoolsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCredConfig(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudIamWorkloadIdentityPoolsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudIamWorkloadIdentityPoolsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudIamWorkloadIdentityPoolsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(GcloudIamWorkloadIdentityPoolsUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudIamWorkloadIdentityPoolsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}