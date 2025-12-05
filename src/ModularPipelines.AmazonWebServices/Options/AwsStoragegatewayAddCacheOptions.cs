using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "add-cache")]
public record AwsStoragegatewayAddCacheOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--disk-ids")] string[] DiskIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}