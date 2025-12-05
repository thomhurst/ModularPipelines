using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "project-info", "update")]
public record GcloudComputeProjectInfoUpdateOptions : GcloudOptions
{
    [CliOption("--default-network-tier")]
    public string? DefaultNetworkTier { get; set; }
}