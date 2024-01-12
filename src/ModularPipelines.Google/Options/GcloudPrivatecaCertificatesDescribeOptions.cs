using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "certificates", "describe")]
public record GcloudPrivatecaCertificatesDescribeOptions(
[property: PositionalArgument] string Certificate,
[property: PositionalArgument] string IssuerLocation,
[property: PositionalArgument] string IssuerPool
) : GcloudOptions;