using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "auto-provisioning-setting", "list")]
public record AzSecurityAutoProvisioningSettingListOptions : AzOptions;