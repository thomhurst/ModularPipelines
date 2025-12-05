using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "create-login-config")]
public record GcloudAnthosCreateLoginConfigOptions(
[property: CliOption("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [CliOption("--merge-from")]
    public string? MergeFrom { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }
}