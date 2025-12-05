using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "update-signal-catalog")]
public record AwsIotfleetwiseUpdateSignalCatalogOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--nodes-to-add")]
    public string[]? NodesToAdd { get; set; }

    [CliOption("--nodes-to-update")]
    public string[]? NodesToUpdate { get; set; }

    [CliOption("--nodes-to-remove")]
    public string[]? NodesToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}