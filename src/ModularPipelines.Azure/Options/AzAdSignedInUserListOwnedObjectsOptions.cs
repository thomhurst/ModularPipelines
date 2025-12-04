using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "signed-in-user", "list-owned-objects")]
public record AzAdSignedInUserListOwnedObjectsOptions : AzOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }
}