using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "auto-provisioning-setting", "list")]
public record AzSecurityAutoProvisioningSettingListOptions : AzOptions;