using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "bindings", "delete")]
public record GcloudResourceManagerTagsBindingsDeleteOptions(
[property: CliOption("--parent")] string Parent,
[property: CliOption("--tag-value")] string TagValue
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}