using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "update-application")]
public record AwsKinesisanalyticsv2UpdateApplicationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CommandSwitch("--application-configuration-update")]
    public string? ApplicationConfigurationUpdate { get; set; }

    [CommandSwitch("--service-execution-role-update")]
    public string? ServiceExecutionRoleUpdate { get; set; }

    [CommandSwitch("--run-configuration-update")]
    public string? RunConfigurationUpdate { get; set; }

    [CommandSwitch("--cloud-watch-logging-option-updates")]
    public string[]? CloudWatchLoggingOptionUpdates { get; set; }

    [CommandSwitch("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}