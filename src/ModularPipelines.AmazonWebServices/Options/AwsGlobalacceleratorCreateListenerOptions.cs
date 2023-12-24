using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "create-listener")]
public record AwsGlobalacceleratorCreateListenerOptions(
[property: CommandSwitch("--accelerator-arn")] string AcceleratorArn,
[property: CommandSwitch("--port-ranges")] string[] PortRanges,
[property: CommandSwitch("--protocol")] string Protocol
) : AwsOptions
{
    [CommandSwitch("--client-affinity")]
    public string? ClientAffinity { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}