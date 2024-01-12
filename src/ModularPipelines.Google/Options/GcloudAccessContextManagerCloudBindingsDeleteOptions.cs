using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "cloud-bindings", "delete")]
public record GcloudAccessContextManagerCloudBindingsDeleteOptions(
[property: CommandSwitch("--binding")] string Binding,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;