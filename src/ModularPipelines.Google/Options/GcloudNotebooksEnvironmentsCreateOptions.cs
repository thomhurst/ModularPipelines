using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "environments", "create")]
public record GcloudNotebooksEnvironmentsCreateOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location,
[property: CliOption("--container-repository")] string ContainerRepository,
[property: CliOption("--container-tag")] string ContainerTag,
[property: CliOption("--vm-image-project")] string VmImageProject,
[property: CliOption("--vm-image-family")] string VmImageFamily,
[property: CliOption("--vm-image-name")] string VmImageName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--post-startup-script")]
    public string? PostStartupScript { get; set; }
}