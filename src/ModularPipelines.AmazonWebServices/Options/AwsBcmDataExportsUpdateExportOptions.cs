using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bcm-data-exports", "update-export")]
public record AwsBcmDataExportsUpdateExportOptions(
[property: CommandSwitch("--export")] string Export,
[property: CommandSwitch("--export-arn")] string ExportArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}