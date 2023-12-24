using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-spaces")]
public record AwsSagemakerListSpacesOptions : AwsOptions
{
    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--domain-id-equals")]
    public string? DomainIdEquals { get; set; }

    [CommandSwitch("--space-name-contains")]
    public string? SpaceNameContains { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}