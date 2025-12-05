using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "replace-all")]
public record GcloudAccessContextManagerPerimetersReplaceAllOptions(
[property: CliArgument] string Policy,
[property: CliOption("--source-file")] string SourceFile
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}