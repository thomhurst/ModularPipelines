using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "replace-all")]
public record GcloudAccessContextManagerPerimetersReplaceAllOptions(
[property: PositionalArgument] string Policy,
[property: CommandSwitch("--source-file")] string SourceFile
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}