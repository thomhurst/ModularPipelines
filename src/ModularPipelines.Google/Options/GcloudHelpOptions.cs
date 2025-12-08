using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("help")]
public record GcloudHelpOptions(
[property: CliArgument] string Command,
[property: CliArgument] string SearchTerms
) : GcloudOptions;