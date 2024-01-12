using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "associated-projects", "add")]
public record GcloudComputeSharedVpcAssociatedProjectsAddOptions(
[property: PositionalArgument] string ProjectId,
[property: CommandSwitch("--host-project")] string HostProject
) : GcloudOptions;