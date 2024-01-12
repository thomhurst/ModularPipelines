using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "dataplane-v2-encryption", "describe")]
public record GcloudContainerHubDataplaneV2EncryptionDescribeOptions : GcloudOptions;