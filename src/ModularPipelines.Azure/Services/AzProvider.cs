using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzProvider
{
    public AzProvider(
        AzProviderOperation operation,
        AzProviderPermission permission,
        ICommand internalCommand
    )
    {
        Operation = operation;
        Permission = permission;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzProviderOperation Operation { get; }

    public AzProviderPermission Permission { get; }

    public async Task<CommandResult> List(AzProviderListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderListOptions(), token);
    }

    public async Task<CommandResult> Register(AzProviderRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzProviderShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unregister(AzProviderUnregisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}