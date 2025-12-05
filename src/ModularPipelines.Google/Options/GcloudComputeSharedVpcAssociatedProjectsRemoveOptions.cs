using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "associated-projects", "remove")]
public record GcloudComputeSharedVpcAssociatedProjectsRemoveOptions(
[property: CliArgument] string ProjectId,
[property: CliOption("--host-project")] string HostProject
) : GcloudOptions;