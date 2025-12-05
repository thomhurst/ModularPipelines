using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "disassociate-elastic-ip")]
public record AwsOpsworksDisassociateElasticIpOptions(
[property: CliOption("--elastic-ip")] string ElasticIp
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}