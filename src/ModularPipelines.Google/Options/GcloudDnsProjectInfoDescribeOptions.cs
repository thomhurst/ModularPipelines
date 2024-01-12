using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "project-info", "describe")]
public record GcloudDnsProjectInfoDescribeOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;