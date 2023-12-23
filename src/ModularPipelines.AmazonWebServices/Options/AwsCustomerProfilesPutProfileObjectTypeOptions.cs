using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "put-profile-object-type")]
public record AwsCustomerProfilesPutProfileObjectTypeOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--object-type-name")] string ObjectTypeName,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--template-id")]
    public string? TemplateId { get; set; }

    [CommandSwitch("--expiration-days")]
    public int? ExpirationDays { get; set; }

    [CommandSwitch("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CommandSwitch("--source-last-updated-timestamp-format")]
    public string? SourceLastUpdatedTimestampFormat { get; set; }

    [CommandSwitch("--fields")]
    public IEnumerable<KeyValue>? Fields { get; set; }

    [CommandSwitch("--keys")]
    public IEnumerable<KeyValue>? Keys { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}