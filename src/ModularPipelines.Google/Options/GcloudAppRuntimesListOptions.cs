using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "runtimes", "list")]
public record GcloudAppRuntimesListOptions(
[property: CommandSwitch("--environment")] string Environment
) : GcloudOptions;