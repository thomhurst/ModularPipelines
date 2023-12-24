using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-export")]
public record AwsLexv2ModelsCreateExportOptions(
[property: CommandSwitch("--resource-specification")] string ResourceSpecification,
[property: CommandSwitch("--file-format")] string FileFormat
) : AwsOptions
{
    [CommandSwitch("--file-password")]
    public string? FilePassword { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}