using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "get-application-assignment-configuration")]
public record AwsSsoAdminGetApplicationAssignmentConfigurationOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}