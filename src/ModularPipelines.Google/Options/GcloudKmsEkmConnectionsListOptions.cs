using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-connections", "list")]
public record GcloudKmsEkmConnectionsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;