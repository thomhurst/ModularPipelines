using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "organizations", "list-host-projects")]
public record GcloudComputeSharedVpcOrganizationsListHostProjectsOptions(
[property: PositionalArgument] string OrganizationId
) : GcloudOptions;