using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "update-membership")]
public record AwsCleanroomsUpdateMembershipOptions(
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CommandSwitch("--query-log-status")]
    public string? QueryLogStatus { get; set; }

    [CommandSwitch("--default-result-configuration")]
    public string? DefaultResultConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}