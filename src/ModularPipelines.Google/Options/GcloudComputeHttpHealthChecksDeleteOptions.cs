using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "http-health-checks", "delete")]
public record GcloudComputeHttpHealthChecksDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;