using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr")]
public class AzAcrConnectedRegistry
{
    public AzAcrConnectedRegistry(
        AzAcrConnectedRegistryInstall install,
        AzAcrConnectedRegistryPermissions permissions,
        ICommand internalCommand
    )
    {
        Install = install;
        Permissions = permissions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAcrConnectedRegistryInstall Install { get; }

    public AzAcrConnectedRegistryPermissions Permissions { get; }

    public async Task<CommandResult> Create(AzAcrConnectedRegistryCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deactivate(AzAcrConnectedRegistryDeactivateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAcrConnectedRegistryDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSettings(AzAcrConnectedRegistryGetSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAcrConnectedRegistryListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClientTokens(AzAcrConnectedRegistryListClientTokensOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Repo(AzAcrConnectedRegistryRepoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAcrConnectedRegistryShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAcrConnectedRegistryUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

