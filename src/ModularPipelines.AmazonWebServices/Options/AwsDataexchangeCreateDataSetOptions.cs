using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "create-data-set")]
public record AwsDataexchangeCreateDataSetOptions(
[property: CommandSwitch("--asset-type")] string AssetType,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}