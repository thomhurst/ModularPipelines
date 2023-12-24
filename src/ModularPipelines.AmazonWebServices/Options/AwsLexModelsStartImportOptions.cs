using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "start-import")]
public record AwsLexModelsStartImportOptions(
[property: CommandSwitch("--payload")] string Payload,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--merge-strategy")] string MergeStrategy
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}