using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "templates", "describe")]
public record GcloudTranscoderTemplatesDescribeOptions(
[property: CliArgument] string TemplateId,
[property: CliArgument] string Location
) : GcloudOptions;