using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "list")]
public record GcloudAiModelsListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}