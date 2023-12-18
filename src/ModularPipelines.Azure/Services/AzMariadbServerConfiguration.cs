using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "server")]
public class AzMariadbServerConfiguration
{
    public AzMariadbServerConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzMariadbServerConfigurationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerConfigurationListOptions(), token);
    }

    public async Task<CommandResult> Set(AzMariadbServerConfigurationSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerConfigurationSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzMariadbServerConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerConfigurationShowOptions(), token);
    }
}