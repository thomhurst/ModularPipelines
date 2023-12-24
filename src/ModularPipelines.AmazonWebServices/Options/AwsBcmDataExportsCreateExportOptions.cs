using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bcm-data-exports", "create-export")]
public record AwsBcmDataExportsCreateExportOptions(
[property: CommandSwitch("--export")] string Export
) : AwsOptions
{
    [CommandSwitch("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}