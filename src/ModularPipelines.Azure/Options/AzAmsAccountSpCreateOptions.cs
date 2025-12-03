using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "account", "sp", "create")]
public record AzAmsAccountSpCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--new-sp-name")]
    public string? NewSpName { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliFlag("--xml")]
    public bool? Xml { get; set; }

    [CliOption("--years")]
    public string? Years { get; set; }
}