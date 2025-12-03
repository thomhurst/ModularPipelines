using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "publish-package-version")]
public record AwsCodeartifactPublishPackageVersionOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--format")] string Format,
[property: CliOption("--package")] string Package,
[property: CliOption("--package-version")] string PackageVersion,
[property: CliOption("--asset-content")] string AssetContent,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--asset-sha256")] string AssetSha256
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}