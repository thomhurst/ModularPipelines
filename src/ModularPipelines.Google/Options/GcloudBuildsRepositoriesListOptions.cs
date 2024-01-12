using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "repositories", "list")]
public record GcloudBuildsRepositoriesListOptions(
[property: CommandSwitch("--connection")] string Connection,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;