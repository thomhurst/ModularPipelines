using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "policy", "export")]
public record GcloudContainerBinauthzPolicyExportOptions : GcloudOptions;