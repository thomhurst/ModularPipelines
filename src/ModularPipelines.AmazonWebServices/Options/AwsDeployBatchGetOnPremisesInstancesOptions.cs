using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "batch-get-on-premises-instances")]
public record AwsDeployBatchGetOnPremisesInstancesOptions(
[property: CliOption("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}