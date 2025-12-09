using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "associate-elastic-ip")]
public record AwsOpsworksAssociateElasticIpOptions(
[property: CliOption("--elastic-ip")] string ElasticIp
) : AwsOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}