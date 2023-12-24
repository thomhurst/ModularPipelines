using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "create-repository")]
public record AwsProtonCreateRepositoryOptions(
[property: CommandSwitch("--connection-arn")] string ConnectionArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider")] string Provider
) : AwsOptions
{
    [CommandSwitch("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}