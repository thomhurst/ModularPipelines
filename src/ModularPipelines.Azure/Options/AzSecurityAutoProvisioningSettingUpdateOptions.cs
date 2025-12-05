using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "auto-provisioning-setting", "update")]
public record AzSecurityAutoProvisioningSettingUpdateOptions(
[property: CliOption("--auto-provision")] string AutoProvision,
[property: CliOption("--name")] string Name
) : AzOptions;