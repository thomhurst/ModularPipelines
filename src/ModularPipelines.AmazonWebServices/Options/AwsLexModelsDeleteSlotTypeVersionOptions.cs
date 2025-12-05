using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "delete-slot-type-version")]
public record AwsLexModelsDeleteSlotTypeVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--slot-type-version")] string SlotTypeVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}