using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "create-tag-option")]
public record AwsServicecatalogCreateTagOptionOptions(
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--value")] string Value
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}