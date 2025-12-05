using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "describe")]
public record GcloudOrganizationsDescribeOptions(
[property: CliArgument] string OrganizationId
) : GcloudOptions;