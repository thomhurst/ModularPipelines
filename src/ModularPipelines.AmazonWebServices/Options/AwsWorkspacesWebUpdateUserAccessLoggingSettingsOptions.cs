using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "update-user-access-logging-settings")]
public record AwsWorkspacesWebUpdateUserAccessLoggingSettingsOptions(
[property: CliOption("--user-access-logging-settings-arn")] string UserAccessLoggingSettingsArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--kinesis-stream-arn")]
    public string? KinesisStreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}