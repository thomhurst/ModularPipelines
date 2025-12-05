using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto")]
public class AzKustoAttachedDatabaseConfiguration
{
    public AzKustoAttachedDatabaseConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoAttachedDatabaseConfigurationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKustoAttachedDatabaseConfigurationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoAttachedDatabaseConfigurationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzKustoAttachedDatabaseConfigurationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKustoAttachedDatabaseConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoAttachedDatabaseConfigurationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoAttachedDatabaseConfigurationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoAttachedDatabaseConfigurationUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKustoAttachedDatabaseConfigurationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoAttachedDatabaseConfigurationWaitOptions(), token);
    }
}