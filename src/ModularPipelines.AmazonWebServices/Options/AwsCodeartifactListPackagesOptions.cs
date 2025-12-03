using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "list-packages")]
public record AwsCodeartifactListPackagesOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--package-prefix")]
    public string? PackagePrefix { get; set; }

    [CliOption("--publish")]
    public string? Publish { get; set; }

    [CliOption("--upstream")]
    public string? Upstream { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}