using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "create-listener")]
public record AwsGlobalacceleratorCreateListenerOptions(
[property: CliOption("--accelerator-arn")] string AcceleratorArn,
[property: CliOption("--port-ranges")] string[] PortRanges,
[property: CliOption("--protocol")] string Protocol
) : AwsOptions
{
    [CliOption("--client-affinity")]
    public string? ClientAffinity { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}