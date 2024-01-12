using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "create-login-config")]
public record GcloudAnthosCreateLoginConfigOptions(
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [CommandSwitch("--merge-from")]
    public string? MergeFrom { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }
}