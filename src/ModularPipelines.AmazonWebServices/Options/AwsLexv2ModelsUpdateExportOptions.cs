using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "update-export")]
public record AwsLexv2ModelsUpdateExportOptions(
[property: CommandSwitch("--export-id")] string ExportId
) : AwsOptions
{
    [CommandSwitch("--file-password")]
    public string? FilePassword { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}