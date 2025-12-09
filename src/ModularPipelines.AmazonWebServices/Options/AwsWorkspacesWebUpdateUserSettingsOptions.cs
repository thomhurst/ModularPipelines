using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "update-user-settings")]
public record AwsWorkspacesWebUpdateUserSettingsOptions(
[property: CliOption("--user-settings-arn")] string UserSettingsArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--cookie-synchronization-configuration")]
    public string? CookieSynchronizationConfiguration { get; set; }

    [CliOption("--copy-allowed")]
    public string? CopyAllowed { get; set; }

    [CliOption("--disconnect-timeout-in-minutes")]
    public int? DisconnectTimeoutInMinutes { get; set; }

    [CliOption("--download-allowed")]
    public string? DownloadAllowed { get; set; }

    [CliOption("--idle-disconnect-timeout-in-minutes")]
    public int? IdleDisconnectTimeoutInMinutes { get; set; }

    [CliOption("--paste-allowed")]
    public string? PasteAllowed { get; set; }

    [CliOption("--print-allowed")]
    public string? PrintAllowed { get; set; }

    [CliOption("--upload-allowed")]
    public string? UploadAllowed { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}