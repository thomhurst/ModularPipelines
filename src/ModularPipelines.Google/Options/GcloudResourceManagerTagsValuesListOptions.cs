using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "values", "list")]
public record GcloudResourceManagerTagsValuesListOptions(
[property: CliOption("--parent")] string Parent
) : GcloudOptions;