using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance")]
public class AzMaintenancePublicConfiguration
{
    public AzMaintenancePublicConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzMaintenancePublicConfigurationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenancePublicConfigurationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMaintenancePublicConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenancePublicConfigurationShowOptions(), token);
    }
}