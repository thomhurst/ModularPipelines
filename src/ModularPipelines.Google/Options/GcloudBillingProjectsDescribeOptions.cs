using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "projects", "describe")]
public record GcloudBillingProjectsDescribeOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;