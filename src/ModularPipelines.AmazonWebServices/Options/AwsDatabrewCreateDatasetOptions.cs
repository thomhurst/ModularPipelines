using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "create-dataset")]
public record AwsDatabrewCreateDatasetOptions(
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

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}