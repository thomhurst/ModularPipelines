using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "project-configs", "describe")]
public record GcloudSourceProjectConfigsDescribeOptions : GcloudOptions;