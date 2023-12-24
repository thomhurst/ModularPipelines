using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "update-data-catalog")]
public record AwsAthenaUpdateDataCatalogOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}