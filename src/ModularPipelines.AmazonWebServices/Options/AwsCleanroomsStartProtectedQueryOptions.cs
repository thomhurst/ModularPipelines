using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "start-protected-query")]
public record AwsCleanroomsStartProtectedQueryOptions(
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--sql-parameters")] string SqlParameters
) : AwsOptions
{
    [CommandSwitch("--result-configuration")]
    public string? ResultConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}