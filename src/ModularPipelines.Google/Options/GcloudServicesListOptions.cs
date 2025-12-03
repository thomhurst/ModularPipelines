using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "list")]
public record GcloudServicesListOptions : GcloudOptions
{
    [CliFlag("--available")]
    public bool? Available { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }
}