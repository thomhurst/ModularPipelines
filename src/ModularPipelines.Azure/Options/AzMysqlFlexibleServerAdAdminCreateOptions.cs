using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "flexible-server", "ad-admin", "create")]
public record AzMysqlFlexibleServerAdAdminCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--identity")] string Identity,
[property: CliOption("--object-id")] string ObjectId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}