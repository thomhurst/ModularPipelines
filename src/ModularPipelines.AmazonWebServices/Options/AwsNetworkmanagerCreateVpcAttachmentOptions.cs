using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-vpc-attachment")]
public record AwsNetworkmanagerCreateVpcAttachmentOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId,
[property: CliOption("--vpc-arn")] string VpcArn,
[property: CliOption("--subnet-arns")] string[] SubnetArns
) : AwsOptions
{
    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}