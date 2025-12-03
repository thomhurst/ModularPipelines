using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "delete")]
public record GcloudComputeInstancesDeleteOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--delete-disks")]
    public string? DeleteDisks { get; set; }

    [CliFlag("all")]
    public bool? All { get; set; }

    [CliFlag("boot")]
    public bool? Boot { get; set; }

    [CliFlag("data")]
    public bool? Data { get; set; }

    [CliOption("--keep-disks")]
    public string? KeepDisks { get; set; }
}