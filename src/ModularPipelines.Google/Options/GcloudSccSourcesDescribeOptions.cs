using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "sources", "describe")]
public record GcloudSccSourcesDescribeOptions(
[property: PositionalArgument] string Organization,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--source-display-name")] string SourceDisplayName
) : GcloudOptions;