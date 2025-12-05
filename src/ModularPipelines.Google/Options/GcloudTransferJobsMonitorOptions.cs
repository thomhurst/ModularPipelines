using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "monitor")]
public record GcloudTransferJobsMonitorOptions(
[property: CliArgument] string Name
) : GcloudOptions;