using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "dataplane-v2-encryption", "enable")]
public record GcloudContainerHubDataplaneV2EncryptionEnableOptions : GcloudOptions;