using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "create-domain")]
public record AwsCustomerProfilesCreateDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--default-expiration-days")] int DefaultExpirationDays
) : AwsOptions
{
    [CliOption("--default-encryption-key")]
    public string? DefaultEncryptionKey { get; set; }

    [CliOption("--dead-letter-queue-url")]
    public string? DeadLetterQueueUrl { get; set; }

    [CliOption("--matching")]
    public string? Matching { get; set; }

    [CliOption("--rule-based-matching")]
    public string? RuleBasedMatching { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}