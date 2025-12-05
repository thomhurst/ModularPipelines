using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "set-named-ports")]
public record GcloudComputeInstanceGroupsSetNamedPortsOptions(
[property: CliArgument] string Name,
[property: CliOption("--named-ports")] string[] NamedPorts
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}