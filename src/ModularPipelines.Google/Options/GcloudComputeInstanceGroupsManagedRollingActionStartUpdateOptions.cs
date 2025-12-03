using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "rolling-action", "start-update")]
public record GcloudComputeInstanceGroupsManagedRollingActionStartUpdateOptions : GcloudOptions
{
    public GcloudComputeInstanceGroupsManagedRollingActionStartUpdateOptions(
        string name,
        string[] version
    )
    {
        Name = name;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Name { get; set; }

    [CliOption("--version")]
    public new string[] Version { get; set; }
}
