using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "create-lake-formation-identity-center-configuration")]
public record AwsLakeformationCreateLakeFormationIdentityCenterConfigurationOptions : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--instance-arn")]
    public string? InstanceArn { get; set; }

    [CommandSwitch("--external-filtering")]
    public string? ExternalFiltering { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}