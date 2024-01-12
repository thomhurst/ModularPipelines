using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcoder", "templates", "delete")]
public record GcloudTranscoderTemplatesDeleteOptions(
[property: PositionalArgument] string TemplateId,
[property: PositionalArgument] string Location
) : GcloudOptions;