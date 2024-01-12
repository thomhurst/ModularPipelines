using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "clusters", "list")]
public record GcloudWorkstationsClustersListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;