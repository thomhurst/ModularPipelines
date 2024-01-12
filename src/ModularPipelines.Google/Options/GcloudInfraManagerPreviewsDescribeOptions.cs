using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "previews", "describe")]
public record GcloudInfraManagerPreviewsDescribeOptions(
[property: PositionalArgument] string Preview,
[property: PositionalArgument] string Location
) : GcloudOptions;