using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "configs", "list")]
public record GcloudEndpointsConfigsListOptions(
[property: CommandSwitch("--service")] string Service
) : GcloudOptions;