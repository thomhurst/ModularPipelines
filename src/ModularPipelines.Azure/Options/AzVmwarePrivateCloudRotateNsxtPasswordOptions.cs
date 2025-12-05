using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "private-cloud", "rotate-nsxt-password")]
public record AzVmwarePrivateCloudRotateNsxtPasswordOptions : AzOptions;