using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "import")]
public record GcloudLookerInstancesImportOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--source-gcs-uri")] string SourceGcsUri
) : GcloudOptions;