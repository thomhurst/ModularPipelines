using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("write-tree")]
[ExcludeFromCodeCoverage]
public record GitWriteTreeOptions : GitOptions
{
    [BooleanCommandSwitch("--missing-ok")]
    public bool? MissingOk { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }
}