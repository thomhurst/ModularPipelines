using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers")]
public class GcloudIamWorkloadIdentityPoolsProvidersKeys
{
    public GcloudIamWorkloadIdentityPoolsProvidersKeys(
        GcloudIamWorkloadIdentityPoolsProvidersKeysOperations operations,
        ICommand internalCommand
    )
    {
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamWorkloadIdentityPoolsProvidersKeysOperations Operations { get; }

    public async Task<CommandResult> Create(GcloudIamWorkloadIdentityPoolsProvidersKeysCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudIamWorkloadIdentityPoolsProvidersKeysDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudIamWorkloadIdentityPoolsProvidersKeysDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudIamWorkloadIdentityPoolsProvidersKeysListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(GcloudIamWorkloadIdentityPoolsProvidersKeysUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}