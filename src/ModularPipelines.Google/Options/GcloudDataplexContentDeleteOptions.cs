using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "content", "delete")]
public record GcloudDataplexContentDeleteOptions(
[property: CliArgument] string Content,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions;