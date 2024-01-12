using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "describe")]
public record GcloudPrivatecaTemplatesDescribeOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions;