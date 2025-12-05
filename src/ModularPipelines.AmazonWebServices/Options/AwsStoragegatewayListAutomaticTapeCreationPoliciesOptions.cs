using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "list-automatic-tape-creation-policies")]
public record AwsStoragegatewayListAutomaticTapeCreationPoliciesOptions : AwsOptions
{
    [CliOption("--gateway-arn")]
    public string? GatewayArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}