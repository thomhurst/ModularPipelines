using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "delete-kx-scaling-group")]
public record AwsFinspaceDeleteKxScalingGroupOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--scaling-group-name")] string ScalingGroupName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}