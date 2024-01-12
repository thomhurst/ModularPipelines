using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "web", "set-iam-policy")]
public record GcloudIapWebSetIamPolicyOptions(
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}