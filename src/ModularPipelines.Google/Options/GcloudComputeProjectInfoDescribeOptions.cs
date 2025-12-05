using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "project-info", "describe")]
public record GcloudComputeProjectInfoDescribeOptions : GcloudOptions;