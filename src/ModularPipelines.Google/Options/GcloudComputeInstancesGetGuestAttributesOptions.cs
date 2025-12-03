using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "get-guest-attributes")]
public record GcloudComputeInstancesGetGuestAttributesOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--query-path")]
    public string? QueryPath { get; set; }
}