using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "set-iam-policy")]
public record GcloudPrivatecaTemplatesSetIamPolicyOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;