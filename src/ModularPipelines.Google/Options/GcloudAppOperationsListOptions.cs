using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "operations", "list")]
public record GcloudAppOperationsListOptions : GcloudOptions
{
    [CliFlag("--pending")]
    public bool? Pending { get; set; }
}