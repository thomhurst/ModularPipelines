using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "notifications", "list")]
public record GcloudSccNotificationsListOptions(
[property: PositionalArgument] string Parent
) : GcloudOptions;