using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhub-config", "describe-home-region-controls")]
public record AwsMigrationhubConfigDescribeHomeRegionControlsOptions : AwsOptions
{
    [CommandSwitch("--control-id")]
    public string? ControlId { get; set; }

    [CommandSwitch("--home-region")]
    public string? HomeRegion { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}