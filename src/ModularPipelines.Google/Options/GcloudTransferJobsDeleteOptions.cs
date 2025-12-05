using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "delete")]
public record GcloudTransferJobsDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;