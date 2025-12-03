using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "replace-all")]
public record GcloudAccessContextManagerLevelsReplaceAllOptions(
[property: CliArgument] string Policy,
[property: CliOption("--source-file")] string SourceFile
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}