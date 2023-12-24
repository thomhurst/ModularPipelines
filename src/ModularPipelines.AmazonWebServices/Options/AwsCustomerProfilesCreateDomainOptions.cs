using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "create-domain")]
public record AwsCustomerProfilesCreateDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--default-expiration-days")] int DefaultExpirationDays
) : AwsOptions
{
    [CommandSwitch("--default-encryption-key")]
    public string? DefaultEncryptionKey { get; set; }

    [CommandSwitch("--dead-letter-queue-url")]
    public string? DeadLetterQueueUrl { get; set; }

    [CommandSwitch("--matching")]
    public string? Matching { get; set; }

    [CommandSwitch("--rule-based-matching")]
    public string? RuleBasedMatching { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}