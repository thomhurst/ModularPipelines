using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "projects", "describe")]
public record GcloudBillingProjectsDescribeOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;