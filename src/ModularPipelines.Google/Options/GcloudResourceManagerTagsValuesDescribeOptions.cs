using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "values", "describe")]
public record GcloudResourceManagerTagsValuesDescribeOptions(
[property: CliArgument] string ResourceName
) : GcloudOptions;