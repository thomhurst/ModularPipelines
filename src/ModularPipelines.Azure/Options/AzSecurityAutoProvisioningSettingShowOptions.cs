using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "auto-provisioning-setting", "show")]
public record AzSecurityAutoProvisioningSettingShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;