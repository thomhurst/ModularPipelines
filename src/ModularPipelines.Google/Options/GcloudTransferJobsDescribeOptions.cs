using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "describe")]
public record GcloudTransferJobsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;