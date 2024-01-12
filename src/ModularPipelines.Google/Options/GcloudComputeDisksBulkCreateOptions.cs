using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "bulk", "create")]
public record GcloudComputeDisksBulkCreateOptions(
[property: CommandSwitch("--source-consistency-group-policy")] string SourceConsistencyGroupPolicy,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions;