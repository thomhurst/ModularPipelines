using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub")]
public class AzNotificationHubCredential
{
    public AzNotificationHubCredential(
        AzNotificationHubCredentialAdm adm,
        AzNotificationHubCredentialApns apns,
        AzNotificationHubCredentialBaidu baidu,
        AzNotificationHubCredentialGcm gcm,
        AzNotificationHubCredentialMpns mpns,
        AzNotificationHubCredentialWns wns,
        ICommand internalCommand
    )
    {
        Adm = adm;
        Apns = apns;
        Baidu = baidu;
        Gcm = gcm;
        Mpns = mpns;
        Wns = wns;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNotificationHubCredentialAdm Adm { get; }

    public AzNotificationHubCredentialApns Apns { get; }

    public AzNotificationHubCredentialBaidu Baidu { get; }

    public AzNotificationHubCredentialGcm Gcm { get; }

    public AzNotificationHubCredentialMpns Mpns { get; }

    public AzNotificationHubCredentialWns Wns { get; }

    public async Task<CommandResult> List(AzNotificationHubCredentialListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}