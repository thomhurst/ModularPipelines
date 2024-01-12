using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "project-info", "describe")]
public record GcloudComputeProjectInfoDescribeOptions : GcloudOptions;