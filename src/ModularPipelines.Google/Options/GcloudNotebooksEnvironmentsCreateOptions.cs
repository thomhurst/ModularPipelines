using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "environments", "create")]
public record GcloudNotebooksEnvironmentsCreateOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--container-repository")] string ContainerRepository,
[property: CommandSwitch("--container-tag")] string ContainerTag,
[property: CommandSwitch("--vm-image-project")] string VmImageProject,
[property: CommandSwitch("--vm-image-family")] string VmImageFamily,
[property: CommandSwitch("--vm-image-name")] string VmImageName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--post-startup-script")]
    public string? PostStartupScript { get; set; }
}