using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "delete-tape")]
public record AwsStoragegatewayDeleteTapeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--tape-arn")] string TapeArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}