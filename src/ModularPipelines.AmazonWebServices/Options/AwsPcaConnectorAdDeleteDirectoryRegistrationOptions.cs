using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "delete-directory-registration")]
public record AwsPcaConnectorAdDeleteDirectoryRegistrationOptions(
[property: CommandSwitch("--directory-registration-arn")] string DirectoryRegistrationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}