using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "associated-projects", "remove")]
public record GcloudComputeSharedVpcAssociatedProjectsRemoveOptions(
[property: PositionalArgument] string ProjectId,
[property: CommandSwitch("--host-project")] string HostProject
) : GcloudOptions;