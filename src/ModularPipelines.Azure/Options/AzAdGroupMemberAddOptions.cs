using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "group", "member", "add")]
public record AzAdGroupMemberAddOptions(
[property: CliOption("--group")] string Group,
[property: CliOption("--member-id")] string MemberId
) : AzOptions;