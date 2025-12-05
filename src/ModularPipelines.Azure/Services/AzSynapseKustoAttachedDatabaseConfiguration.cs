using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kusto")]
public class AzSynapseKustoAttachedDatabaseConfiguration
{
    public AzSynapseKustoAttachedDatabaseConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSynapseKustoAttachedDatabaseConfigurationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSynapseKustoAttachedDatabaseConfigurationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoAttachedDatabaseConfigurationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSynapseKustoAttachedDatabaseConfigurationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSynapseKustoAttachedDatabaseConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoAttachedDatabaseConfigurationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSynapseKustoAttachedDatabaseConfigurationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoAttachedDatabaseConfigurationUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSynapseKustoAttachedDatabaseConfigurationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoAttachedDatabaseConfigurationWaitOptions(), token);
    }
}