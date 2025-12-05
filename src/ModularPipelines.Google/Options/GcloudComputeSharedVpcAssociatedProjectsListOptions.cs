using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "associated-projects", "list")]
public record GcloudComputeSharedVpcAssociatedProjectsListOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;