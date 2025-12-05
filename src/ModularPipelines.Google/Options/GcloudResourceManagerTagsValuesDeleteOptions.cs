using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "values", "delete")]
public record GcloudResourceManagerTagsValuesDeleteOptions(
[property: CliArgument] string ResourceName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}