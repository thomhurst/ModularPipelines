using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "previews", "list")]
public record GcloudInfraManagerPreviewsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;