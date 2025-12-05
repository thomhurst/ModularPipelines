using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "list-associated-resources")]
public record GcloudComputeSharedVpcListAssociatedResourcesOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;