using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "delete-package")]
public record AwsCodeartifactDeletePackageOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--format")] string Format,
[property: CliOption("--package")] string Package
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}