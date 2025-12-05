using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "cloud-bindings", "list")]
public record GcloudAccessContextManagerCloudBindingsListOptions : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}