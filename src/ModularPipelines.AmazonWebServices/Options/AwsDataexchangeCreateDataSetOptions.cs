using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "create-data-set")]
public record AwsDataexchangeCreateDataSetOptions(
[property: CliOption("--asset-type")] string AssetType,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}