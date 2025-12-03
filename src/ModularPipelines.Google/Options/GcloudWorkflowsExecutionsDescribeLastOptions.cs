using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "executions", "describe-last")]
public record GcloudWorkflowsExecutionsDescribeLastOptions : GcloudOptions;