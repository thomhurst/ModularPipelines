using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "content", "delete")]
public record GcloudDataplexContentDeleteOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;