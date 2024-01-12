using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "certificates", "update")]
public record GcloudCertificateManagerCertificatesUpdateOptions(
[property: PositionalArgument] string Certificate,
[property: PositionalArgument] string Location
) : GcloudOptions;