using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "update-domain")]
public record AwsCustomerProfilesUpdateDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--default-expiration-days")]
    public int? DefaultExpirationDays { get; set; }

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