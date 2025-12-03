using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "organizations", "list-host-projects")]
public record GcloudComputeSharedVpcOrganizationsListHostProjectsOptions(
[property: CliArgument] string OrganizationId
) : GcloudOptions;