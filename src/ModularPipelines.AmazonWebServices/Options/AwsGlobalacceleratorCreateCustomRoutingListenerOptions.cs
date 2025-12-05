using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "create-custom-routing-listener")]
public record AwsGlobalacceleratorCreateCustomRoutingListenerOptions(
[property: CliOption("--accelerator-arn")] string AcceleratorArn,
[property: CliOption("--port-ranges")] string[] PortRanges
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}