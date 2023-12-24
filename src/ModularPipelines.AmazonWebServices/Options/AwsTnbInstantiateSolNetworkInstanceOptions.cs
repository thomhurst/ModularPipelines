using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "instantiate-sol-network-instance")]
public record AwsTnbInstantiateSolNetworkInstanceOptions(
[property: CommandSwitch("--ns-instance-id")] string NsInstanceId
) : AwsOptions
{
    [CommandSwitch("--additional-params-for-ns")]
    public string? AdditionalParamsForNs { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}