using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "jobs", "monitor")]
public record GcloudTransferJobsMonitorOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;