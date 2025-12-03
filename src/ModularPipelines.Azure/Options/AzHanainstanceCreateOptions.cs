using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hanainstance", "create")]
public record AzHanainstanceCreateOptions(
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--location")] string Location,
[property: CliOption("--os-computer-name")] string OsComputerName,
[property: CliOption("--partner-node-id")] string PartnerNodeId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--ssh-public-key")] string SshPublicKey
) : AzOptions;