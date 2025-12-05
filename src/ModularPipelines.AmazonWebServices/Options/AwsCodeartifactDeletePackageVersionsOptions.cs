using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "delete-package-versions")]
public record AwsCodeartifactDeletePackageVersionsOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--format")] string Format,
[property: CliOption("--package")] string Package,
[property: CliOption("--versions")] string[] Versions
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--expected-status")]
    public string? ExpectedStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}