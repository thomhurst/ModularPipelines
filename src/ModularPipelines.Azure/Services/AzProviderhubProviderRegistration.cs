using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub")]
public class AzProviderhubProviderRegistration
{
    public AzProviderhubProviderRegistration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzProviderhubProviderRegistrationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzProviderhubProviderRegistrationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubProviderRegistrationDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GenerateOperation(AzProviderhubProviderRegistrationGenerateOperationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubProviderRegistrationGenerateOperationOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzProviderhubProviderRegistrationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubProviderRegistrationListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzProviderhubProviderRegistrationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubProviderRegistrationShowOptions(), cancellationToken: token);
    }
}