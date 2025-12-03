using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "sources", "describe")]
public record GcloudSccSourcesDescribeOptions(
[property: CliArgument] string Organization,
[property: CliOption("--source")] string Source,
[property: CliOption("--source-display-name")] string SourceDisplayName
) : GcloudOptions;