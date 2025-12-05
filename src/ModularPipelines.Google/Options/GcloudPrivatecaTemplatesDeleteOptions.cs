using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "delete")]
public record GcloudPrivatecaTemplatesDeleteOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location
) : GcloudOptions;