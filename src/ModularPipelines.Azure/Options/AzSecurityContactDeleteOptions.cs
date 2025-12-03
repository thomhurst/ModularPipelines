using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "contact", "delete")]
public record AzSecurityContactDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions;