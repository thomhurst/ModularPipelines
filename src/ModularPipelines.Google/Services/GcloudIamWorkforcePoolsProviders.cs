using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools")]
public class GcloudIamWorkforcePoolsProviders
{
    public GcloudIamWorkforcePoolsProviders(
        GcloudIamWorkforcePoolsProvidersKeys keys,
        GcloudIamWorkforcePoolsProvidersOperations operations,
        ICommand internalCommand
    )
    {
        Keys = keys;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamWorkforcePoolsProvidersKeys Keys { get; }

    public GcloudIamWorkforcePoolsProvidersOperations Operations { get; }

    public async Task<CommandResult> CreateOidc(GcloudIamWorkforcePoolsProvidersCreateOidcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSaml(GcloudIamWorkforcePoolsProvidersCreateSamlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudIamWorkforcePoolsProvidersDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudIamWorkforcePoolsProvidersDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudIamWorkforcePoolsProvidersListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(GcloudIamWorkforcePoolsProvidersUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOidc(GcloudIamWorkforcePoolsProvidersUpdateOidcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSaml(GcloudIamWorkforcePoolsProvidersUpdateSamlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}