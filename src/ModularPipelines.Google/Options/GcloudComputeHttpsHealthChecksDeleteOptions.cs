using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "https-health-checks", "delete")]
public record GcloudComputeHttpsHealthChecksDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;