using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "certificate", "generate-verification-code")]
public record AzIotHubCertificateGenerateVerificationCodeOptions(
[property: CliOption("--etag")] string Etag,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}