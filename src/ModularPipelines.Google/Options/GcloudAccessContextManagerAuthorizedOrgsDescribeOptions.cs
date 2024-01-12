using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "authorized-orgs", "describe")]
public record GcloudAccessContextManagerAuthorizedOrgsDescribeOptions(
[property: PositionalArgument] string AuthorizedOrgsDesc,
[property: PositionalArgument] string Policy
) : GcloudOptions;