using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "list")]
public record GcloudOrganizationsListOptions : GcloudOptions;