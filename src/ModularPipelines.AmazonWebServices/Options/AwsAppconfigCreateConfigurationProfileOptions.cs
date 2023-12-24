using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "create-configuration-profile")]
public record AwsAppconfigCreateConfigurationProfileOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--location-uri")] string LocationUri
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--retrieval-role-arn")]
    public string? RetrievalRoleArn { get; set; }

    [CommandSwitch("--validators")]
    public string[]? Validators { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}