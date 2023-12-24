using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "update-repository-link")]
public record AwsCodestarConnectionsUpdateRepositoryLinkOptions(
[property: CommandSwitch("--repository-link-id")] string RepositoryLinkId
) : AwsOptions
{
    [CommandSwitch("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CommandSwitch("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}