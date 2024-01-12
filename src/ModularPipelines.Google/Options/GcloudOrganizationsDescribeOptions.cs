using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "describe")]
public record GcloudOrganizationsDescribeOptions(
[property: PositionalArgument] string OrganizationId
) : GcloudOptions;