using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "group", "member", "check")]
public record AzAdGroupMemberCheckOptions(
[property: CliOption("--group")] string Group,
[property: CliOption("--member-id")] string MemberId
) : AzOptions;