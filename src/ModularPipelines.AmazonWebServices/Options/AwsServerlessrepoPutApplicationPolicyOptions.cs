using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("serverlessrepo", "put-application-policy")]
public record AwsServerlessrepoPutApplicationPolicyOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--statements")] string[] Statements
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}