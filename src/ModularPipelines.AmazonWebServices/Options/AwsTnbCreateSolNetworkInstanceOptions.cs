using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tnb", "create-sol-network-instance")]
public record AwsTnbCreateSolNetworkInstanceOptions(
[property: CommandSwitch("--ns-name")] string NsName,
[property: CommandSwitch("--nsd-info-id")] string NsdInfoId
) : AwsOptions
{
    [CommandSwitch("--ns-description")]
    public string? NsDescription { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}