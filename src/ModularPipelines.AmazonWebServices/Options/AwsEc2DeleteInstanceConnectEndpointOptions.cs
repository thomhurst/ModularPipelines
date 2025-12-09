using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-instance-connect-endpoint")]
public record AwsEc2DeleteInstanceConnectEndpointOptions(
[property: CliOption("--instance-connect-endpoint-id")] string InstanceConnectEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}