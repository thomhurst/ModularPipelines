using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "bulk", "create")]
public record GcloudComputeDisksBulkCreateOptions(
[property: CliOption("--source-consistency-group-policy")] string SourceConsistencyGroupPolicy,
[property: CliOption("--region")] string Region,
[property: CliOption("--zone")] string Zone
) : GcloudOptions;