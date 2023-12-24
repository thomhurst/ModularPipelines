using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "get-application-grant")]
public record AwsSsoAdminGetApplicationGrantOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn,
[property: CommandSwitch("--grant-type")] string GrantType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}