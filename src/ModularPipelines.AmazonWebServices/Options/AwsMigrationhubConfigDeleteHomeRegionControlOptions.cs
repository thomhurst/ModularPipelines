using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhub-config", "delete-home-region-control")]
public record AwsMigrationhubConfigDeleteHomeRegionControlOptions(
[property: CommandSwitch("--control-id")] string ControlId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}