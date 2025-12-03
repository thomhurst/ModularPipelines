using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "event-types", "list")]
public record GcloudFunctionsEventTypesListOptions : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }
}