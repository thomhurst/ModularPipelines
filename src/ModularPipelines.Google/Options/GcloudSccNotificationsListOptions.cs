using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "notifications", "list")]
public record GcloudSccNotificationsListOptions(
[property: CliArgument] string Parent
) : GcloudOptions;