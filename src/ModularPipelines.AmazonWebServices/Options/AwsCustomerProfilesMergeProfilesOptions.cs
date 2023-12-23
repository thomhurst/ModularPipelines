using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "merge-profiles")]
public record AwsCustomerProfilesMergeProfilesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--main-profile-id")] string MainProfileId,
[property: CommandSwitch("--profile-ids-to-be-merged")] string[] ProfileIdsToBeMerged
) : AwsOptions
{
    [CommandSwitch("--field-source-profile-ids")]
    public string? FieldSourceProfileIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}