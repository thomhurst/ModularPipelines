using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "create-custom-routing-listener")]
public record AwsGlobalacceleratorCreateCustomRoutingListenerOptions(
[property: CommandSwitch("--accelerator-arn")] string AcceleratorArn,
[property: CommandSwitch("--port-ranges")] string[] PortRanges
) : AwsOptions
{
    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}