using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "list-associated-resources")]
public record GcloudComputeSharedVpcListAssociatedResourcesOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;