using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "get-shielded-identity")]
public record GcloudComputeInstancesGetShieldedIdentityOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}