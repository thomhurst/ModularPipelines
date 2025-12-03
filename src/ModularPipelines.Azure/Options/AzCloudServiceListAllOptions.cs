using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-service", "list-all")]
public record AzCloudServiceListAllOptions : AzOptions;