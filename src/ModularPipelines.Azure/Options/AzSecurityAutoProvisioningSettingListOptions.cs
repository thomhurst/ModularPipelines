using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "auto-provisioning-setting", "list")]
public record AzSecurityAutoProvisioningSettingListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}