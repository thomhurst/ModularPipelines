using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("csvmware", "vm-template", "list")]
public record AzCsvmwareVmTemplateListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-pool")] string ResourcePool
) : AzOptions;