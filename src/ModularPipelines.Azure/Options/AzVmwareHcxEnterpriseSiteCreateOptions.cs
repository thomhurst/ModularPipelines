using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "hcx-enterprise-site", "create")]
public record AzVmwareHcxEnterpriseSiteCreateOptions(
[property: CliOption("--hcx-enterprise-site-name")] string HcxEnterpriseSiteName,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;