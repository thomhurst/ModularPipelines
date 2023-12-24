using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "start-import")]
public record AwsLexv2ModelsStartImportOptions(
[property: CommandSwitch("--import-id")] string ImportId,
[property: CommandSwitch("--resource-specification")] string ResourceSpecification,
[property: CommandSwitch("--merge-strategy")] string MergeStrategy
) : AwsOptions
{
    [CommandSwitch("--file-password")]
    public string? FilePassword { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}