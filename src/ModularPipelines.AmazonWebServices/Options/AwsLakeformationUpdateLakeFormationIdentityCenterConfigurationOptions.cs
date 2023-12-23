using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "update-lake-formation-identity-center-configuration")]
public record AwsLakeformationUpdateLakeFormationIdentityCenterConfigurationOptions : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--application-status")]
    public string? ApplicationStatus { get; set; }

    [CommandSwitch("--external-filtering")]
    public string? ExternalFiltering { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}