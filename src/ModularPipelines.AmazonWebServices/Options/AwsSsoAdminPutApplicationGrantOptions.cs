using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "put-application-grant")]
public record AwsSsoAdminPutApplicationGrantOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn,
[property: CommandSwitch("--grant")] string Grant,
[property: CommandSwitch("--grant-type")] string GrantType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}