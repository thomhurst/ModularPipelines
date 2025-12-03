using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "create-sol-network-instance")]
public record AwsTnbCreateSolNetworkInstanceOptions(
[property: CliOption("--ns-name")] string NsName,
[property: CliOption("--nsd-info-id")] string NsdInfoId
) : AwsOptions
{
    [CliOption("--ns-description")]
    public string? NsDescription { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}