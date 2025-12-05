using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "previews", "describe")]
public record GcloudInfraManagerPreviewsDescribeOptions(
[property: CliArgument] string Preview,
[property: CliArgument] string Location
) : GcloudOptions;