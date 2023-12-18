using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzIdentity
{
    public AzIdentity(
        AzIdentityFederatedCredential federatedCredential,
        ICommand internalCommand
    )
    {
        FederatedCredential = federatedCredential;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIdentityFederatedCredential FederatedCredential { get; }

    public async Task<CommandResult> Create(AzIdentityCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIdentityDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIdentityDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzIdentityListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIdentityListOptions(), token);
    }

    public async Task<CommandResult> ListOperations(AzIdentityListOperationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIdentityListOperationsOptions(), token);
    }

    public async Task<CommandResult> ListResources(AzIdentityListResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIdentityListResourcesOptions(), token);
    }

    public async Task<CommandResult> Show(AzIdentityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIdentityShowOptions(), token);
    }
}