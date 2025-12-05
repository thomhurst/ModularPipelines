using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "rbacrolebindings", "create")]
public record GcloudContainerHubScopesRbacrolebindingsCreateOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location,
[property: CliArgument] string Scope,
[property: CliOption("--role")] string Role,
[property: CliOption("--group")] string Group,
[property: CliOption("--user")] string User
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}