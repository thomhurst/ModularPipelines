using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "bindings", "create")]
public record GcloudResourceManagerTagsBindingsCreateOptions(
[property: CliOption("--parent")] string Parent,
[property: CliOption("--tag-value")] string TagValue
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}