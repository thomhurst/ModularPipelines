using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "previews", "list")]
public record GcloudInfraManagerPreviewsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;