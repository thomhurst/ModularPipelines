using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "dataplane-v2-encryption", "describe")]
public record GcloudContainerFleetDataplaneV2EncryptionDescribeOptions : GcloudOptions;