using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "describe-entity")]
public record AwsWorkmailDescribeEntityOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--email")] string Email
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}