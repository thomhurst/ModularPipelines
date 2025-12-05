using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "kms-configs", "describe")]
public record GcloudNetappKmsConfigsDescribeOptions(
[property: CliArgument] string KmsConfig,
[property: CliArgument] string Location
) : GcloudOptions;