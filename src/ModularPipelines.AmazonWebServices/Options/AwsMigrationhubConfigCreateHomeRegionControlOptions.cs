using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhub-config", "create-home-region-control")]
public record AwsMigrationhubConfigCreateHomeRegionControlOptions(
[property: CommandSwitch("--home-region")] string HomeRegion,
[property: CommandSwitch("--target")] string Target
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}