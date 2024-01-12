using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "policycontroller", "enable")]
public record GcloudContainerHubPolicycontrollerEnableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CommandSwitch("--memberships")]
    public string[]? Memberships { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--audit-interval")]
    public string? AuditInterval { get; set; }

    [CommandSwitch("--constraint-violation-limit")]
    public string? ConstraintViolationLimit { get; set; }

    [BooleanCommandSwitch("--no-default-bundles")]
    public bool? NoDefaultBundles { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [BooleanCommandSwitch("--clear-exemptable-namespaces")]
    public bool? ClearExemptableNamespaces { get; set; }

    [CommandSwitch("--exemptable-namespaces")]
    public string? ExemptableNamespaces { get; set; }

    [BooleanCommandSwitch("--log-denies")]
    public bool? LogDenies { get; set; }

    [BooleanCommandSwitch("--no-log-denies")]
    public bool? NoLogDenies { get; set; }

    [CommandSwitch("--monitoring")]
    public string? Monitoring { get; set; }

    [BooleanCommandSwitch("--no-monitoring")]
    public bool? NoMonitoring { get; set; }

    [BooleanCommandSwitch("--mutation")]
    public bool? Mutation { get; set; }

    [BooleanCommandSwitch("--no-mutation")]
    public bool? NoMutation { get; set; }

    [BooleanCommandSwitch("--referential-rules")]
    public bool? ReferentialRules { get; set; }

    [BooleanCommandSwitch("--no-referential-rules")]
    public bool? NoReferentialRules { get; set; }

    [CommandSwitch("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }

    [BooleanCommandSwitch("--no-fleet-default-member-config")]
    public bool? NoFleetDefaultMemberConfig { get; set; }
}