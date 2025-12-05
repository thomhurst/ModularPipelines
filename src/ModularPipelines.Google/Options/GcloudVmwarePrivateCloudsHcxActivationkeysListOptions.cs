using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "hcx", "activationkeys", "list")]
public record GcloudVmwarePrivateCloudsHcxActivationkeysListOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--location")] string Location
) : GcloudOptions;