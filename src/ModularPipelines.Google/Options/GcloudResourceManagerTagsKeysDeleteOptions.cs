using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "keys", "delete")]
public record GcloudResourceManagerTagsKeysDeleteOptions(
[property: CliArgument] string ResourceName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}