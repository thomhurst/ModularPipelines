using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "update-user-access-logging-settings")]
public record AwsWorkspacesWebUpdateUserAccessLoggingSettingsOptions(
[property: CommandSwitch("--user-access-logging-settings-arn")] string UserAccessLoggingSettingsArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--kinesis-stream-arn")]
    public string? KinesisStreamArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}