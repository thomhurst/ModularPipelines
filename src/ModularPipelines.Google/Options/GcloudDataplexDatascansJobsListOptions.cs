using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "jobs", "list")]
public record GcloudDataplexDatascansJobsListOptions(
[property: CommandSwitch("--datascan")] string Datascan,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;