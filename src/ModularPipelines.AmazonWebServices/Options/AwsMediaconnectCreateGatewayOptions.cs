using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "create-gateway")]
public record AwsMediaconnectCreateGatewayOptions(
[property: CliOption("--egress-cidr-blocks")] string[] EgressCidrBlocks,
[property: CliOption("--name")] string Name,
[property: CliOption("--networks")] string[] Networks
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}