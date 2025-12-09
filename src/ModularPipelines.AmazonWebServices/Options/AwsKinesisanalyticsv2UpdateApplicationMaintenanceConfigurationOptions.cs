using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "update-application-maintenance-configuration")]
public record AwsKinesisanalyticsv2UpdateApplicationMaintenanceConfigurationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--application-maintenance-configuration-update")] string ApplicationMaintenanceConfigurationUpdate
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}