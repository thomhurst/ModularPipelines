using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "update-user-settings")]
public record AwsWorkspacesWebUpdateUserSettingsOptions(
[property: CommandSwitch("--user-settings-arn")] string UserSettingsArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--cookie-synchronization-configuration")]
    public string? CookieSynchronizationConfiguration { get; set; }

    [CommandSwitch("--copy-allowed")]
    public string? CopyAllowed { get; set; }

    [CommandSwitch("--disconnect-timeout-in-minutes")]
    public int? DisconnectTimeoutInMinutes { get; set; }

    [CommandSwitch("--download-allowed")]
    public string? DownloadAllowed { get; set; }

    [CommandSwitch("--idle-disconnect-timeout-in-minutes")]
    public int? IdleDisconnectTimeoutInMinutes { get; set; }

    [CommandSwitch("--paste-allowed")]
    public string? PasteAllowed { get; set; }

    [CommandSwitch("--print-allowed")]
    public string? PrintAllowed { get; set; }

    [CommandSwitch("--upload-allowed")]
    public string? UploadAllowed { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}