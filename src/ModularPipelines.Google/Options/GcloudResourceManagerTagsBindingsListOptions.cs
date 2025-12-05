using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "bindings", "list")]
public record GcloudResourceManagerTagsBindingsListOptions(
[property: CliOption("--parent")] string Parent
) : GcloudOptions
{
    [CliFlag("--effective")]
    public bool? Effective { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}