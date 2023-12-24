using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "create-tapes")]
public record AwsStoragegatewayCreateTapesOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--tape-size-in-bytes")] long TapeSizeInBytes,
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--num-tapes-to-create")] int NumTapesToCreate,
[property: CommandSwitch("--tape-barcode-prefix")] string TapeBarcodePrefix
) : AwsOptions
{
    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--pool-id")]
    public string? PoolId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}