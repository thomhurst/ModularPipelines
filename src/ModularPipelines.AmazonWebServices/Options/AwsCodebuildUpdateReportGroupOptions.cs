using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "update-report-group")]
public record AwsCodebuildUpdateReportGroupOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--export-config")]
    public string? ExportConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}