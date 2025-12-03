using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "group", "member", "list")]
public record AzAdGroupMemberListOptions(
[property: CliOption("--group")] string Group
) : AzOptions;