using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "environments", "list")]
public record GcloudDataplexEnvironmentsListOptions(
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;