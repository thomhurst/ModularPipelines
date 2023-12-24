using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "update-sol-network-instance")]
public record AwsTnbUpdateSolNetworkInstanceOptions(
[property: CommandSwitch("--ns-instance-id")] string NsInstanceId,
[property: CommandSwitch("--update-type")] string UpdateType
) : AwsOptions
{
    [CommandSwitch("--modify-vnf-info-data")]
    public string? ModifyVnfInfoData { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}