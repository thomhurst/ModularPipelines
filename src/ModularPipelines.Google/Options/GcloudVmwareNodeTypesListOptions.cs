using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "node-types", "list")]
public record GcloudVmwareNodeTypesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}