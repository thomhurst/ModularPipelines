using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "change-resource-record-sets")]
public record AwsRoute53ChangeResourceRecordSetsOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId,
[property: CommandSwitch("--change-batch")] string ChangeBatch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}