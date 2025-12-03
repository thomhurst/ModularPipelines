using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "namespaces", "get-credentials")]
public record GcloudContainerFleetScopesNamespacesGetCredentialsOptions(
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliOption("--membership")]
    public string? Membership { get; set; }

    [CliOption("--membership-location")]
    public string? MembershipLocation { get; set; }

    [CliOption("--set-namespace-in-config")]
    public string? SetNamespaceInConfig { get; set; }
}