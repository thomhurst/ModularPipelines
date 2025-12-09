using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack", "sub", "delete")]
public record AzStackSubDeleteOptions : AzOptions
{
    [CliFlag("--delete-all")]
    public bool? DeleteAll { get; set; }

    [CliFlag("--delete-resource-groups")]
    public bool? DeleteResourceGroups { get; set; }

    [CliFlag("--delete-resources")]
    public bool? DeleteResources { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}