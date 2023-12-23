using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "search")]
public record AwsDatazoneSearchOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--search-scope")] string SearchScope
) : AwsOptions
{
    [CommandSwitch("--additional-attributes")]
    public string[]? AdditionalAttributes { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--owning-project-identifier")]
    public string? OwningProjectIdentifier { get; set; }

    [CommandSwitch("--search-in")]
    public string[]? SearchIn { get; set; }

    [CommandSwitch("--search-text")]
    public string? SearchText { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}