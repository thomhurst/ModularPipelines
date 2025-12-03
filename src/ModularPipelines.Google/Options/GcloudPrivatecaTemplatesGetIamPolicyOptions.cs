using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "get-iam-policy")]
public record GcloudPrivatecaTemplatesGetIamPolicyOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location
) : GcloudOptions;