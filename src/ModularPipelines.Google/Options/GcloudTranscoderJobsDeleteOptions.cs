using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "jobs", "delete")]
public record GcloudTranscoderJobsDeleteOptions(
[property: CliArgument] string JobName,
[property: CliArgument] string Location
) : GcloudOptions;