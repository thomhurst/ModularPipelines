using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp")]
public class AzWebappConfig
{
    public AzWebappConfig(
        AzWebappConfigAccessRestriction accessRestriction,
        AzWebappConfigAppsettings appsettings,
        AzWebappConfigBackup backup,
        AzWebappConfigConnectionString connectionString,
        AzWebappConfigContainer container,
        AzWebappConfigHostname hostname,
        AzWebappConfigSnapshot snapshot,
        AzWebappConfigSsl ssl,
        AzWebappConfigStorageAccount storageAccount,
        ICommand internalCommand
    )
    {
        AccessRestriction = accessRestriction;
        Appsettings = appsettings;
        Backup = backup;
        ConnectionString = connectionString;
        Container = container;
        Hostname = hostname;
        Snapshot = snapshot;
        Ssl = ssl;
        StorageAccount = storageAccount;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebappConfigAccessRestriction AccessRestriction { get; }

    public AzWebappConfigAppsettings Appsettings { get; }

    public AzWebappConfigBackup Backup { get; }

    public AzWebappConfigConnectionString ConnectionString { get; }

    public AzWebappConfigContainer Container { get; }

    public AzWebappConfigHostname Hostname { get; }

    public AzWebappConfigSnapshot Snapshot { get; }

    public AzWebappConfigSsl Ssl { get; }

    public AzWebappConfigStorageAccount StorageAccount { get; }

    public async Task<CommandResult> Set(AzWebappConfigSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappConfigShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigShowOptions(), token);
    }
}