using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "update-application")]
public record AwsKinesisanalyticsv2UpdateApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CliOption("--application-configuration-update")]
    public string? ApplicationConfigurationUpdate { get; set; }

    [CliOption("--service-execution-role-update")]
    public string? ServiceExecutionRoleUpdate { get; set; }

    [CliOption("--run-configuration-update")]
    public string? RunConfigurationUpdate { get; set; }

    [CliOption("--cloud-watch-logging-option-updates")]
    public string[]? CloudWatchLoggingOptionUpdates { get; set; }

    [CliOption("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}