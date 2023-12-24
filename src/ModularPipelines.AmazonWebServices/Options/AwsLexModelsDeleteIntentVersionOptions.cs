using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "delete-intent-version")]
public record AwsLexModelsDeleteIntentVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--intent-version")] string IntentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}