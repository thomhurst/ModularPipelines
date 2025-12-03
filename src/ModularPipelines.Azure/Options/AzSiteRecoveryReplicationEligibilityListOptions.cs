using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery", "replication-eligibility", "list")]
public record AzSiteRecoveryReplicationEligibilityListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtual-machine-name")] string VirtualMachineName
) : AzOptions;