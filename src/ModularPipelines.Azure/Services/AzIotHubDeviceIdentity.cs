using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub")]
public class AzIotHubDeviceIdentity
{
    public AzIotHubDeviceIdentity(
        AzIotHubDeviceIdentityChildren children,
        AzIotHubDeviceIdentityConnectionString connectionString,
        AzIotHubDeviceIdentityParent parent,
        ICommand internalCommand
    )
    {
        Children = children;
        ConnectionString = connectionString;
        Parent = parent;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotHubDeviceIdentityChildren Children { get; }

    public AzIotHubDeviceIdentityConnectionString ConnectionString { get; }

    public AzIotHubDeviceIdentityParent Parent { get; }

    public async Task<CommandResult> Create(AzIotHubDeviceIdentityCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotHubDeviceIdentityDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(AzIotHubDeviceIdentityExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzIotHubDeviceIdentityImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotHubDeviceIdentityListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RenewKey(AzIotHubDeviceIdentityRenewKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotHubDeviceIdentityShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotHubDeviceIdentityUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}