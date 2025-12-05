using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "export")]
public record GcloudLookerInstancesExportOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--kms-key")] string KmsKey,
[property: CliOption("--target-gcs-uri")] string TargetGcsUri
) : GcloudOptions;