using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workflows", "executions", "wait-last")]
public record GcloudWorkflowsExecutionsWaitLastOptions : GcloudOptions;