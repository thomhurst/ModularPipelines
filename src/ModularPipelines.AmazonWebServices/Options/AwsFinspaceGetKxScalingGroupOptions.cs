using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "get-kx-scaling-group")]
public record AwsFinspaceGetKxScalingGroupOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--scaling-group-name")] string ScalingGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}