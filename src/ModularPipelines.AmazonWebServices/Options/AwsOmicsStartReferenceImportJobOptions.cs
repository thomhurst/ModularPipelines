using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "start-reference-import-job")]
public record AwsOmicsStartReferenceImportJobOptions(
[property: CliOption("--reference-store-id")] string ReferenceStoreId,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--sources")] string[] Sources
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}