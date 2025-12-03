using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "certificates", "describe")]
public record GcloudPrivatecaCertificatesDescribeOptions(
[property: CliArgument] string Certificate,
[property: CliArgument] string IssuerLocation,
[property: CliArgument] string IssuerPool
) : GcloudOptions;