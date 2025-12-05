using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "update-listener")]
public record AwsGlobalacceleratorUpdateListenerOptions(
[property: CliOption("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CliOption("--port-ranges")]
    public string[]? PortRanges { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--client-affinity")]
    public string? ClientAffinity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}