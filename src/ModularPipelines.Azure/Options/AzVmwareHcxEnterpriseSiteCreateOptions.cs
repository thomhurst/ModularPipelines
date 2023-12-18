using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "hcx-enterprise-site", "create")]
public record AzVmwareHcxEnterpriseSiteCreateOptions(
[property: CommandSwitch("--hcx-enterprise-site-name")] string HcxEnterpriseSiteName,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;