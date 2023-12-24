using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "describe-conformance-pack-compliance")]
public record AwsConfigserviceDescribeConformancePackComplianceOptions(
[property: CommandSwitch("--conformance-pack-name")] string ConformancePackName
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}