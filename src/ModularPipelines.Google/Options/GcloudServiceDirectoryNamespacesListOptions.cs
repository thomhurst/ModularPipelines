using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "namespaces", "list")]
public record GcloudServiceDirectoryNamespacesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;