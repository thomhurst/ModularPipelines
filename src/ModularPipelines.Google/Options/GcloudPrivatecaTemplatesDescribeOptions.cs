using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "describe")]
public record GcloudPrivatecaTemplatesDescribeOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location
) : GcloudOptions;