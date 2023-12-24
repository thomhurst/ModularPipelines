using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "login")]
public record AwsCodeartifactLoginOptions(
[property: CommandSwitch("--tool")] string AwsCodeTool,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--repository")] string Repository
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}