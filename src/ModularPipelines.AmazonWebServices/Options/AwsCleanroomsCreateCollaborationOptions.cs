using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "create-collaboration")]
public record AwsCleanroomsCreateCollaborationOptions(
[property: CommandSwitch("--members")] string[] Members,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--creator-member-abilities")] string[] CreatorMemberAbilities,
[property: CommandSwitch("--creator-display-name")] string CreatorDisplayName,
[property: CommandSwitch("--query-log-status")] string QueryLogStatus
) : AwsOptions
{
    [CommandSwitch("--data-encryption-metadata")]
    public string? DataEncryptionMetadata { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--creator-payment-configuration")]
    public string? CreatorPaymentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}