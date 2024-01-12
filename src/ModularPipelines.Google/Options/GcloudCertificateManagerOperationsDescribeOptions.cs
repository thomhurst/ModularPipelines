using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "operations", "describe")]
public record GcloudCertificateManagerOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location
) : GcloudOptions;