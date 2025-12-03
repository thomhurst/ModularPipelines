using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("confcom", "acipolicygen")]
public record AzConfcomAcipolicygenOptions : AzOptions
{
    [CliFlag("--approve-wildcards")]
    public bool? ApproveWildcards { get; set; }

    [CliFlag("--debug-mode")]
    public bool? DebugMode { get; set; }

    [CliFlag("--diff")]
    public bool? Diff { get; set; }

    [CliFlag("--disable-stdio")]
    public bool? DisableStdio { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--infrastructure-svn")]
    public string? InfrastructureSvn { get; set; }

    [CliOption("--input")]
    public string? Input { get; set; }

    [CliFlag("--outraw")]
    public bool? Outraw { get; set; }

    [CliFlag("--outraw-pretty-print")]
    public bool? OutrawPrettyPrint { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliFlag("--print-existing-policy")]
    public bool? PrintExistingPolicy { get; set; }

    [CliFlag("--print-policy")]
    public bool? PrintPolicy { get; set; }

    [CliOption("--save-to-file")]
    public string? SaveToFile { get; set; }

    [CliOption("--tar")]
    public string? Tar { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliFlag("--validate-sidecar")]
    public bool? ValidateSidecar { get; set; }
}