using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "dbconnection", "create")]
public record AzStaticwebappDbconnectionCreateOptions(
[property: CliOption("--db-resource-id")] string DbResourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}