using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "kms-configs", "describe")]
public record GcloudNetappKmsConfigsDescribeOptions(
[property: PositionalArgument] string KmsConfig,
[property: PositionalArgument] string Location
) : GcloudOptions;