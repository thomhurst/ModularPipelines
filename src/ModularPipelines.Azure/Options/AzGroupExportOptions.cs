using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "export")]
public record AzGroupExportOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--include-comments")]
    public bool? IncludeComments { get; set; }

    [CliFlag("--include-parameter-default-value")]
    public bool? IncludeParameterDefaultValue { get; set; }

    [CliOption("--resource-ids")]
    public string? ResourceIds { get; set; }

    [CliFlag("--skip-all-params")]
    public bool? SkipAllParams { get; set; }

    [CliFlag("--skip-resource-name-params")]
    public bool? SkipResourceNameParams { get; set; }
}