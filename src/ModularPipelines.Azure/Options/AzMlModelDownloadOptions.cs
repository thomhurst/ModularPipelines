using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "download")]
public record AzMlModelDownloadOptions(
[property: CommandSwitch("--model-id")] string ModelId,
[property: CommandSwitch("--target-dir")] string TargetDir
) : AzOptions
{
    [CommandSwitch("--overwrite")]
    public string? Overwrite { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}