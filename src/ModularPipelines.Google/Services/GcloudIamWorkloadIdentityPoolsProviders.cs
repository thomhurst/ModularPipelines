using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools")]
public class GcloudIamWorkloadIdentityPoolsProviders
{
    public GcloudIamWorkloadIdentityPoolsProviders(
        GcloudIamWorkloadIdentityPoolsProvidersKeys keys,
        GcloudIamWorkloadIdentityPoolsProvidersOperations operations,
        ICommand internalCommand
    )
    {
        Keys = keys;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamWorkloadIdentityPoolsProvidersKeys Keys { get; }

    public GcloudIamWorkloadIdentityPoolsProvidersOperations Operations { get; }

    public async Task<CommandResult> CreateAws(GcloudIamWorkloadIdentityPoolsProvidersCreateAwsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOidc(GcloudIamWorkloadIdentityPoolsProvidersCreateOidcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSaml(GcloudIamWorkloadIdentityPoolsProvidersCreateSamlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudIamWorkloadIdentityPoolsProvidersDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudIamWorkloadIdentityPoolsProvidersDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudIamWorkloadIdentityPoolsProvidersListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(GcloudIamWorkloadIdentityPoolsProvidersUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAws(GcloudIamWorkloadIdentityPoolsProvidersUpdateAwsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOidc(GcloudIamWorkloadIdentityPoolsProvidersUpdateOidcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSaml(GcloudIamWorkloadIdentityPoolsProvidersUpdateSamlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}