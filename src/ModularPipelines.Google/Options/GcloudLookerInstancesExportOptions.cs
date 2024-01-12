using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("looker", "instances", "export")]
public record GcloudLookerInstancesExportOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--kms-key")] string KmsKey,
[property: CommandSwitch("--target-gcs-uri")] string TargetGcsUri
) : GcloudOptions;