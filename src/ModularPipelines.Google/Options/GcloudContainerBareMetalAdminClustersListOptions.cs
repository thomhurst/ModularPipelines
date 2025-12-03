using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "admin-clusters", "list")]
public record GcloudContainerBareMetalAdminClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}