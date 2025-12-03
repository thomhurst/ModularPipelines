using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "policycontroller", "content", "bundles", "set")]
public record GcloudContainerHubPolicycontrollerContentBundlesSetOptions(
[property: CliArgument] string BundleName
) : GcloudOptions
{
    [CliFlag("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CliOption("--memberships")]
    public string[]? Memberships { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--exempted-namespaces")]
    public string? ExemptedNamespaces { get; set; }

    [CliFlag("--no-exempted-namespaces")]
    public bool? NoExemptedNamespaces { get; set; }
}