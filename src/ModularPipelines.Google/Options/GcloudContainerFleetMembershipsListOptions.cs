using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "list")]
public record GcloudContainerFleetMembershipsListOptions : GcloudOptions;