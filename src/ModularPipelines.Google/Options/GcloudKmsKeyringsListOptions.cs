using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keyrings", "list")]
public record GcloudKmsKeyringsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;