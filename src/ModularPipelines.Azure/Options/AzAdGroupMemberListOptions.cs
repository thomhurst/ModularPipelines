using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "group", "member", "list")]
public record AzAdGroupMemberListOptions(
[property: CliOption("--group")] string Group
) : AzOptions;