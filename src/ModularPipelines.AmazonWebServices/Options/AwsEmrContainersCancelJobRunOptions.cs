using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-containers", "cancel-job-run")]
public record AwsEmrContainersCancelJobRunOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--virtual-cluster-id")] string VirtualClusterId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}