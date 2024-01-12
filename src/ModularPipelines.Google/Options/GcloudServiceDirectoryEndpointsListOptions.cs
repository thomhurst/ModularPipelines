using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "endpoints", "list")]
public record GcloudServiceDirectoryEndpointsListOptions(
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--namespace")] string Namespace
) : GcloudOptions;