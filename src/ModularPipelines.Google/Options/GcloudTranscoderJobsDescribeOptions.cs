using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcoder", "jobs", "describe")]
public record GcloudTranscoderJobsDescribeOptions(
[property: PositionalArgument] string JobName,
[property: PositionalArgument] string Location
) : GcloudOptions;