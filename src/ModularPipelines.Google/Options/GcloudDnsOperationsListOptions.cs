using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "operations", "list")]
public record GcloudDnsOperationsListOptions(
[property: CliOption("--zones")] string[] Zones
) : GcloudOptions;