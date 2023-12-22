using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "auto-provisioning-setting", "update")]
public record AzSecurityAutoProvisioningSettingUpdateOptions(
[property: CommandSwitch("--auto-provision")] string AutoProvision,
[property: CommandSwitch("--name")] string Name
) : AzOptions;