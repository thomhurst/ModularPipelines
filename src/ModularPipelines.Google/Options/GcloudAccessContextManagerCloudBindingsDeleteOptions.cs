using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "cloud-bindings", "delete")]
public record GcloudAccessContextManagerCloudBindingsDeleteOptions(
[property: CliOption("--binding")] string Binding,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;