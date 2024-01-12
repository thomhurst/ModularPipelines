using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "service-agent")]
public record GcloudStorageServiceAgentOptions : GcloudOptions
{
    [CommandSwitch("--authorize-cmek")]
    public string? AuthorizeCmek { get; set; }
}