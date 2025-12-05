using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "executions", "wait-last")]
public record GcloudWorkflowsExecutionsWaitLastOptions : GcloudOptions;