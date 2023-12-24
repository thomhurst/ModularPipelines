using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "get-slot-type")]
public record AwsLexModelsGetSlotTypeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--slot-type-version")] string SlotTypeVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}