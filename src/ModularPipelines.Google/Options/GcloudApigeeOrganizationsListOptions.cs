using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "organizations", "list")]
public record GcloudApigeeOrganizationsListOptions : GcloudOptions;