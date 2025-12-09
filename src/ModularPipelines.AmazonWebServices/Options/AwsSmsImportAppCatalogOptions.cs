using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "import-app-catalog")]
public record AwsSmsImportAppCatalogOptions : AwsOptions
{
    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}