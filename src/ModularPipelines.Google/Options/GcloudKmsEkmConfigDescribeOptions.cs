using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-config", "describe")]
public record GcloudKmsEkmConfigDescribeOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;