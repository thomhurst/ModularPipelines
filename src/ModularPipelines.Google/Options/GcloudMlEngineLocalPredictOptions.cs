using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "local", "predict")]
public record GcloudMlEngineLocalPredictOptions(
[property: CommandSwitch("--model-dir")] string ModelDir,
[property: CommandSwitch("--json-instances")] string JsonInstances,
[property: CommandSwitch("--json-request")] string JsonRequest,
[property: CommandSwitch("--text-instances")] string TextInstances
) : GcloudOptions
{
    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [CommandSwitch("--signature-name")]
    public string? SignatureName { get; set; }
}