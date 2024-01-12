using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "delete")]
public record GcloudPrivatecaTemplatesDeleteOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions;