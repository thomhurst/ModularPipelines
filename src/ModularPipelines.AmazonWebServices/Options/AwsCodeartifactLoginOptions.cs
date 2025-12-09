using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "login")]
public record AwsCodeartifactLoginOptions(
[property: CliOption("--tool")] string AwsCodeTool,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }
}