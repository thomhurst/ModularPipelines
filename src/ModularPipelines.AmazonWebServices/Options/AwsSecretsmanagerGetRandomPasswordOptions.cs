using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "get-random-password")]
public record AwsSecretsmanagerGetRandomPasswordOptions : AwsOptions
{
    [CommandSwitch("--password-length")]
    public long? PasswordLength { get; set; }

    [CommandSwitch("--exclude-characters")]
    public string? ExcludeCharacters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}