using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "update")]
public record AzStaticwebappUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}