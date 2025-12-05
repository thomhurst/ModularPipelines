using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "create-data-lake")]
public record AwsSecuritylakeCreateDataLakeOptions(
[property: CliOption("--configurations")] string[] Configurations,
[property: CliOption("--meta-store-manager-role-arn")] string MetaStoreManagerRoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}