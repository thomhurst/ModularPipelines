using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "update-application-maintenance-configuration")]
public record AwsKinesisanalyticsv2UpdateApplicationMaintenanceConfigurationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--application-maintenance-configuration-update")] string ApplicationMaintenanceConfigurationUpdate
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}