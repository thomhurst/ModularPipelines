using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "hcx", "activationkeys", "describe")]
public record GcloudVmwarePrivateCloudsHcxActivationkeysDescribeOptions(
[property: CliArgument] string HcxActivationKey,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions;