using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "create-user-settings")]
public record AwsWorkspacesWebCreateUserSettingsOptions(
[property: CommandSwitch("--copy-allowed")] string CopyAllowed,
[property: CommandSwitch("--download-allowed")] string DownloadAllowed,
[property: CommandSwitch("--paste-allowed")] string PasteAllowed,
[property: CommandSwitch("--print-allowed")] string PrintAllowed,
[property: CommandSwitch("--upload-allowed")] string UploadAllowed
) : AwsOptions
{
    [CommandSwitch("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--cookie-synchronization-configuration")]
    public string? CookieSynchronizationConfiguration { get; set; }

    [CommandSwitch("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CommandSwitch("--disconnect-timeout-in-minutes")]
    public int? DisconnectTimeoutInMinutes { get; set; }

    [CommandSwitch("--idle-disconnect-timeout-in-minutes")]
    public int? IdleDisconnectTimeoutInMinutes { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}