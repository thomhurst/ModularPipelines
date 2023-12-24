using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "delete-application-grant")]
public record AwsSsoAdminDeleteApplicationGrantOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn,
[property: CommandSwitch("--grant-type")] string GrantType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}