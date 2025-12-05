using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-view")]
public record AwsConnectDescribeViewOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--view-id")] string ViewId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}