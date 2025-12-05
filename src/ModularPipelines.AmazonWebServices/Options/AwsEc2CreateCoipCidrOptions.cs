using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-coip-cidr")]
public record AwsEc2CreateCoipCidrOptions(
[property: CliOption("--cidr")] string Cidr,
[property: CliOption("--coip-pool-id")] string CoipPoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}