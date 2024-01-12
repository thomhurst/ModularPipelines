using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcoder", "jobs", "delete")]
public record GcloudTranscoderJobsDeleteOptions(
[property: PositionalArgument] string JobName,
[property: PositionalArgument] string Location
) : GcloudOptions;