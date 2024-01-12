using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "get-iam-policy")]
public record GcloudPrivatecaTemplatesGetIamPolicyOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions;