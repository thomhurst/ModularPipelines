using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "create-tape-with-barcode")]
public record AwsStoragegatewayCreateTapeWithBarcodeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--tape-size-in-bytes")] long TapeSizeInBytes,
[property: CliOption("--tape-barcode")] string TapeBarcode
) : AwsOptions
{
    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--pool-id")]
    public string? PoolId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}