using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "delete-slot-type-version")]
public record AwsLexModelsDeleteSlotTypeVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--slot-type-version")] string SlotTypeVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}