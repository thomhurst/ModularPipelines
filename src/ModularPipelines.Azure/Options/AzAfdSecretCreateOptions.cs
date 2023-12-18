using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "secret", "create")]
public record AzAfdSecretCreateOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secret-name")] string SecretName,
[property: CommandSwitch("--secret-source")] string SecretSource
) : AzOptions
{
    [CommandSwitch("--secret-version")]
    public string? SecretVersion { get; set; }

    [BooleanCommandSwitch("--use-latest-version")]
    public bool? UseLatestVersion { get; set; }
}

