using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "create-collaboration")]
public record AwsCleanroomsCreateCollaborationOptions(
[property: CliOption("--members")] string[] Members,
[property: CliOption("--name")] string Name,
[property: CliOption("--description")] string Description,
[property: CliOption("--creator-member-abilities")] string[] CreatorMemberAbilities,
[property: CliOption("--creator-display-name")] string CreatorDisplayName,
[property: CliOption("--query-log-status")] string QueryLogStatus
) : AwsOptions
{
    [CliOption("--data-encryption-metadata")]
    public string? DataEncryptionMetadata { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--creator-payment-configuration")]
    public string? CreatorPaymentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}