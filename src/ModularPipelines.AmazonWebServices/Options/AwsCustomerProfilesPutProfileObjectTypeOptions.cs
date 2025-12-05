using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "put-profile-object-type")]
public record AwsCustomerProfilesPutProfileObjectTypeOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--object-type-name")] string ObjectTypeName,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--template-id")]
    public string? TemplateId { get; set; }

    [CliOption("--expiration-days")]
    public int? ExpirationDays { get; set; }

    [CliOption("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CliOption("--source-last-updated-timestamp-format")]
    public string? SourceLastUpdatedTimestampFormat { get; set; }

    [CliOption("--fields")]
    public IEnumerable<KeyValue>? Fields { get; set; }

    [CliOption("--keys")]
    public IEnumerable<KeyValue>? Keys { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}