using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "policycontroller", "enable")]
public record GcloudContainerFleetPolicycontrollerEnableOptions : GcloudOptions
{
    [CliFlag("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CliOption("--memberships")]
    public string[]? Memberships { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--audit-interval")]
    public string? AuditInterval { get; set; }

    [CliOption("--constraint-violation-limit")]
    public string? ConstraintViolationLimit { get; set; }

    [CliFlag("--no-default-bundles")]
    public bool? NoDefaultBundles { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliFlag("--clear-exemptable-namespaces")]
    public bool? ClearExemptableNamespaces { get; set; }

    [CliOption("--exemptable-namespaces")]
    public string? ExemptableNamespaces { get; set; }

    [CliFlag("--log-denies")]
    public bool? LogDenies { get; set; }

    [CliFlag("--no-log-denies")]
    public bool? NoLogDenies { get; set; }

    [CliOption("--monitoring")]
    public string? Monitoring { get; set; }

    [CliFlag("--no-monitoring")]
    public bool? NoMonitoring { get; set; }

    [CliFlag("--mutation")]
    public bool? Mutation { get; set; }

    [CliFlag("--no-mutation")]
    public bool? NoMutation { get; set; }

    [CliFlag("--referential-rules")]
    public bool? ReferentialRules { get; set; }

    [CliFlag("--no-referential-rules")]
    public bool? NoReferentialRules { get; set; }

    [CliOption("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }

    [CliFlag("--no-fleet-default-member-config")]
    public bool? NoFleetDefaultMemberConfig { get; set; }
}