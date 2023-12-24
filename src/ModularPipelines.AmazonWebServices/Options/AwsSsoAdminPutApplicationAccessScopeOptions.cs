using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "put-application-access-scope")]
public record AwsSsoAdminPutApplicationAccessScopeOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn,
[property: CommandSwitch("--scope")] string Scope
) : AwsOptions
{
    [CommandSwitch("--authorized-targets")]
    public string[]? AuthorizedTargets { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}