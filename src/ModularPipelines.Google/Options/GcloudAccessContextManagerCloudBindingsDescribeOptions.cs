using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "cloud-bindings", "describe")]
public record GcloudAccessContextManagerCloudBindingsDescribeOptions(
[property: CommandSwitch("--binding")] string Binding,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;