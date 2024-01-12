using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "hcx", "activationkeys", "describe")]
public record GcloudVmwarePrivateCloudsHcxActivationkeysDescribeOptions(
[property: PositionalArgument] string HcxActivationKey,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions;