using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "associated-projects", "list")]
public record GcloudComputeSharedVpcAssociatedProjectsListOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;