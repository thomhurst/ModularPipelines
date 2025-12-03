using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "create-user-settings")]
public record AwsWorkspacesWebCreateUserSettingsOptions(
[property: CliOption("--copy-allowed")] string CopyAllowed,
[property: CliOption("--download-allowed")] string DownloadAllowed,
[property: CliOption("--paste-allowed")] string PasteAllowed,
[property: CliOption("--print-allowed")] string PrintAllowed,
[property: CliOption("--upload-allowed")] string UploadAllowed
) : AwsOptions
{
    [CliOption("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--cookie-synchronization-configuration")]
    public string? CookieSynchronizationConfiguration { get; set; }

    [CliOption("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CliOption("--disconnect-timeout-in-minutes")]
    public int? DisconnectTimeoutInMinutes { get; set; }

    [CliOption("--idle-disconnect-timeout-in-minutes")]
    public int? IdleDisconnectTimeoutInMinutes { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}