using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "policy", "export")]
public record GcloudContainerBinauthzPolicyExportOptions : GcloudOptions;