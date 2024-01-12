using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "indexes", "list")]
public record GcloudAiIndexesListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}