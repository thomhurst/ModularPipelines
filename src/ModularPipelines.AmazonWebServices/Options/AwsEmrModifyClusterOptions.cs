using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "modify-cluster")]
public record AwsEmrModifyClusterOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CommandSwitch("--step-concurrency-level")]
    public int? StepConcurrencyLevel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}