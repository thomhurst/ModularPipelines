using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance")]
public class AzMaintenanceConfiguration
{
    public AzMaintenanceConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMaintenanceConfigurationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMaintenanceConfigurationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceConfigurationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMaintenanceConfigurationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceConfigurationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMaintenanceConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceConfigurationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMaintenanceConfigurationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceConfigurationUpdateOptions(), token);
    }
}