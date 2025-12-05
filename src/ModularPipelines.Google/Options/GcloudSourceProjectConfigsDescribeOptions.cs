using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "project-configs", "describe")]
public record GcloudSourceProjectConfigsDescribeOptions : GcloudOptions;