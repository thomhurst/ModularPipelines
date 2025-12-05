using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "jobs", "describe")]
public record GcloudTranscoderJobsDescribeOptions(
[property: CliArgument] string JobName,
[property: CliArgument] string Location
) : GcloudOptions;