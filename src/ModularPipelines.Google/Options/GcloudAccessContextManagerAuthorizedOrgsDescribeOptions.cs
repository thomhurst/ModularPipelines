using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "authorized-orgs", "describe")]
public record GcloudAccessContextManagerAuthorizedOrgsDescribeOptions(
[property: CliArgument] string AuthorizedOrgsDesc,
[property: CliArgument] string Policy
) : GcloudOptions;