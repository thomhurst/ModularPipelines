using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "get-protected-query")]
public record AwsCleanroomsGetProtectedQueryOptions(
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--protected-query-identifier")] string ProtectedQueryIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}