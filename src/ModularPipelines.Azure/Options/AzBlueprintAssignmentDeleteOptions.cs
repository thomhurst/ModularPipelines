using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "assignment", "delete")]
public record AzBlueprintAssignmentDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--delete-behavior")]
    public string? DeleteBehavior { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}