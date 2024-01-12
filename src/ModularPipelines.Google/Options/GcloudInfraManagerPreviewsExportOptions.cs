using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "previews", "export")]
public record GcloudInfraManagerPreviewsExportOptions(
[property: PositionalArgument] string Preview,
[property: PositionalArgument] string Location
) : GcloudOptions;