using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "exists")]
public record AzGroupExistsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;