using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconvert", "list-presets")]
public record AwsMediaconvertListPresetsOptions : AwsOptions
{
    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--list-by")]
    public string? ListBy { get; set; }

    [CommandSwitch("--order")]
    public string? Order { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}