using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "ssl-certificates", "create")]
public record GcloudComputeSslCertificatesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--domains")] string[] Domains,
[property: CliOption("--certificate")] string Certificate,
[property: CliOption("--private-key")] string PrivateKey
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}