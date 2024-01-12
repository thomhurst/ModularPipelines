using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "list")]
public record GcloudServiceDirectoryServicesListOptions(
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;