using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "project-info", "update")]
public record GcloudComputeProjectInfoUpdateOptions : GcloudOptions
{
    [CommandSwitch("--default-network-tier")]
    public string? DefaultNetworkTier { get; set; }
}