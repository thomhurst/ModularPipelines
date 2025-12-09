using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "reject-predictions")]
public record AwsDatazoneRejectPredictionsOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--reject-choices")]
    public string[]? RejectChoices { get; set; }

    [CliOption("--reject-rule")]
    public string? RejectRule { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}