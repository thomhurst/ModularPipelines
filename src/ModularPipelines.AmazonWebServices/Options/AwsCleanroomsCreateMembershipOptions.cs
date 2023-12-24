using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "create-membership")]
public record AwsCleanroomsCreateMembershipOptions(
[property: CommandSwitch("--collaboration-identifier")] string CollaborationIdentifier,
[property: CommandSwitch("--query-log-status")] string QueryLogStatus
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--default-result-configuration")]
    public string? DefaultResultConfiguration { get; set; }

    [CommandSwitch("--payment-configuration")]
    public string? PaymentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}