using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "policycontroller", "content", "bundles", "set")]
public record GcloudContainerFleetPolicycontrollerContentBundlesSetOptions(
[property: PositionalArgument] string BundleName
) : GcloudOptions
{
    [BooleanCommandSwitch("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CommandSwitch("--memberships")]
    public string[]? Memberships { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--exempted-namespaces")]
    public string? ExemptedNamespaces { get; set; }

    [BooleanCommandSwitch("--no-exempted-namespaces")]
    public bool? NoExemptedNamespaces { get; set; }
}