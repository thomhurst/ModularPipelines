using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "create-report-group")]
public record AwsCodebuildCreateReportGroupOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--export-config")] string ExportConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}