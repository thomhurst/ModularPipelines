using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "start-on-demand-audit-task")]
public record AwsIotStartOnDemandAuditTaskOptions(
[property: CliOption("--target-check-names")] string[] TargetCheckNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}