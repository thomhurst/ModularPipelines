using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "update-dataset")]
public record AwsDatabrewUpdateDatasetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--input")] string Input
) : AwsOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--format-options")]
    public string? FormatOptions { get; set; }

    [CommandSwitch("--path-options")]
    public string? PathOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}