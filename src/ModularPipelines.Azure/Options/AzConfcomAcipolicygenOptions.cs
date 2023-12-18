using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confcom", "acipolicygen")]
public record AzConfcomAcipolicygenOptions(
[property: CommandSwitch("--yaml")] string Yaml
) : AzOptions
{
    [BooleanCommandSwitch("--approve-wildcards")]
    public bool? ApproveWildcards { get; set; }

    [BooleanCommandSwitch("--debug-mode")]
    public bool? DebugMode { get; set; }

    [BooleanCommandSwitch("--diff")]
    public bool? Diff { get; set; }

    [BooleanCommandSwitch("--disable-stdio")]
    public bool? DisableStdio { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--infrastructure-svn")]
    public string? InfrastructureSvn { get; set; }

    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [BooleanCommandSwitch("--outraw")]
    public bool? Outraw { get; set; }

    [BooleanCommandSwitch("--outraw-pretty-print")]
    public bool? OutrawPrettyPrint { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [BooleanCommandSwitch("--print-existing-policy")]
    public bool? PrintExistingPolicy { get; set; }

    [BooleanCommandSwitch("--print-policy")]
    public bool? PrintPolicy { get; set; }

    [CommandSwitch("--save-to-file")]
    public string? SaveToFile { get; set; }

    [CommandSwitch("--tar")]
    public string? Tar { get; set; }

    [CommandSwitch("--template-file")]
    public string? TemplateFile { get; set; }

    [BooleanCommandSwitch("--validate-sidecar")]
    public bool? ValidateSidecar { get; set; }
}

