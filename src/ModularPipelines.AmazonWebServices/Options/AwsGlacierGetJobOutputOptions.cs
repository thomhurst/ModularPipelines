using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "get-job-output")]
public record AwsGlacierGetJobOutputOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--range")]
    public string? Range { get; set; }
}