using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "update-sol-network-instance")]
public record AwsTnbUpdateSolNetworkInstanceOptions(
[property: CliOption("--ns-instance-id")] string NsInstanceId,
[property: CliOption("--update-type")] string UpdateType
) : AwsOptions
{
    [CliOption("--modify-vnf-info-data")]
    public string? ModifyVnfInfoData { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}