using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "associate-source-network-stack")]
public record AwsDrsAssociateSourceNetworkStackOptions(
[property: CliOption("--cfn-stack-name")] string CfnStackName,
[property: CliOption("--source-network-id")] string SourceNetworkId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}