using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "create-studio")]
public record AwsNimbleCreateStudioOptions(
[property: CommandSwitch("--admin-role-arn")] string AdminRoleArn,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--studio-name")] string StudioName,
[property: CommandSwitch("--user-role-arn")] string UserRoleArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--studio-encryption-configuration")]
    public string? StudioEncryptionConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}