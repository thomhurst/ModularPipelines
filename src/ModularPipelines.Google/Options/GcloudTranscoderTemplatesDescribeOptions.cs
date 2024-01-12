using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcoder", "templates", "describe")]
public record GcloudTranscoderTemplatesDescribeOptions(
[property: PositionalArgument] string TemplateId,
[property: PositionalArgument] string Location
) : GcloudOptions;