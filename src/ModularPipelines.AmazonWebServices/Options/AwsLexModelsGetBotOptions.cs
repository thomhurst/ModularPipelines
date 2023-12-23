using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "get-bot")]
public record AwsLexModelsGetBotOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version-or-alias")] string VersionOrAlias
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}