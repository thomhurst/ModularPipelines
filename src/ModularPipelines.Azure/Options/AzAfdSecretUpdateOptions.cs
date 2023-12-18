using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "secret", "update")]
public record AzAfdSecretUpdateOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret-name")]
    public string? SecretName { get; set; }

    [CommandSwitch("--secret-source")]
    public string? SecretSource { get; set; }

    [CommandSwitch("--secret-version")]
    public string? SecretVersion { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--use-latest-version")]
    public bool? UseLatestVersion { get; set; }
}