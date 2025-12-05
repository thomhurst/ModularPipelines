using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "get-repository-permissions-policy")]
public record AwsCodeartifactGetRepositoryPermissionsPolicyOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}