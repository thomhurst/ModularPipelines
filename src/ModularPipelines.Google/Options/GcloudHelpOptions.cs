using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("help")]
public record GcloudHelpOptions(
[property: PositionalArgument] string Command,
[property: PositionalArgument] string SearchTerms
) : GcloudOptions;