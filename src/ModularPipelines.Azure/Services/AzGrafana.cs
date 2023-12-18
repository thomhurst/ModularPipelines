using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzGrafana
{
    public AzGrafana(
        AzGrafanaApiKey apiKey,
        AzGrafanaDashboard dashboard,
        AzGrafanaDataSource dataSource,
        AzGrafanaFolder folder,
        AzGrafanaNotificationChannel notificationChannel,
        AzGrafanaServiceAccount serviceAccount,
        AzGrafanaUser user,
        ICommand internalCommand
    )
    {
        ApiKey = apiKey;
        Dashboard = dashboard;
        DataSource = dataSource;
        Folder = folder;
        NotificationChannel = notificationChannel;
        ServiceAccount = serviceAccount;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzGrafanaApiKey ApiKey { get; }

    public AzGrafanaDashboard Dashboard { get; }

    public AzGrafanaDataSource DataSource { get; }

    public AzGrafanaFolder Folder { get; }

    public AzGrafanaNotificationChannel NotificationChannel { get; }

    public AzGrafanaServiceAccount ServiceAccount { get; }

    public AzGrafanaUser User { get; }

    public async Task<CommandResult> Backup(AzGrafanaBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzGrafanaCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzGrafanaDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzGrafanaListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGrafanaListOptions(), token);
    }

    public async Task<CommandResult> Restore(AzGrafanaRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzGrafanaShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzGrafanaUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}