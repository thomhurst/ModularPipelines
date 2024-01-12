using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workflows", "executions", "describe-last")]
public record GcloudWorkflowsExecutionsDescribeLastOptions : GcloudOptions;