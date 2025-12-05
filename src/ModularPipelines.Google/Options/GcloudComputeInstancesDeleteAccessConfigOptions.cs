using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "delete-access-config")]
public record GcloudComputeInstancesDeleteAccessConfigOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--access-config-name")]
    public string? AccessConfigName { get; set; }

    [CliOption("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}