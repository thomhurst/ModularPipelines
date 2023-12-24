using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-compliance-details-by-resource")]
public record AwsConfigserviceGetComplianceDetailsByResourceOptions : AwsOptions
{
    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--compliance-types")]
    public string[]? ComplianceTypes { get; set; }

    [CommandSwitch("--resource-evaluation-id")]
    public string? ResourceEvaluationId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}