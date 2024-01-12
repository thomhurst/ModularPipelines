using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "hcx", "activationkeys", "list")]
public record GcloudVmwarePrivateCloudsHcxActivationkeysListOptions(
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;