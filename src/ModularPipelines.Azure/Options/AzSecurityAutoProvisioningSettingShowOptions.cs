using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "auto-provisioning-setting", "show")]
public record AzSecurityAutoProvisioningSettingShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;