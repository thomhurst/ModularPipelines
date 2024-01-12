using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "hcx", "activationkeys", "create")]
public record GcloudVmwarePrivateCloudsHcxActivationkeysCreateOptions(
[property: PositionalArgument] string HcxActivationKey,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}