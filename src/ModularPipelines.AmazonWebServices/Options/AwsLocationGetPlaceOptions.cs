using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "get-place")]
public record AwsLocationGetPlaceOptions(
[property: CommandSwitch("--index-name")] string IndexName,
[property: CommandSwitch("--place-id")] string PlaceId
) : AwsOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}