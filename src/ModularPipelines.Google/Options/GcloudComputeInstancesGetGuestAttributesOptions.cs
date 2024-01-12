using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "get-guest-attributes")]
public record GcloudComputeInstancesGetGuestAttributesOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--query-path")]
    public string? QueryPath { get; set; }
}