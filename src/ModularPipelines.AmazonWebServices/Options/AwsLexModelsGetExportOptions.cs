using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "get-export")]
public record AwsLexModelsGetExportOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--export-type")] string ExportType,
[property: CommandSwitch("--resource-version")] string ResourceVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}