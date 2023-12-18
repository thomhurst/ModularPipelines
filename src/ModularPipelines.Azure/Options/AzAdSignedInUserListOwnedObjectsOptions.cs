using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "signed-in-user", "list-owned-objects")]
public record AzAdSignedInUserListOwnedObjectsOptions : AzOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }
}