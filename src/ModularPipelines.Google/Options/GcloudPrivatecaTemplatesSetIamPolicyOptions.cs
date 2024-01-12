using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "set-iam-policy")]
public record GcloudPrivatecaTemplatesSetIamPolicyOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;