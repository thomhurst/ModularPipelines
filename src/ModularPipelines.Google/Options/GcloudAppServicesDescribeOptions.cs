using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "services", "describe")]
public record GcloudAppServicesDescribeOptions(
[property: PositionalArgument] string Service
) : GcloudOptions;