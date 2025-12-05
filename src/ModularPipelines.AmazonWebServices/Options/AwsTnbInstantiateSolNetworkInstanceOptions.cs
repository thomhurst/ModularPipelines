using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "instantiate-sol-network-instance")]
public record AwsTnbInstantiateSolNetworkInstanceOptions(
[property: CliOption("--ns-instance-id")] string NsInstanceId
) : AwsOptions
{
    [CliOption("--additional-params-for-ns")]
    public string? AdditionalParamsForNs { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}