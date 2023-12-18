using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "configure")]
public record AzDevopsConfigureOptions : AzOptions
{
    [CommandSwitch("--defaults")]
    public string? Defaults { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("--use-git-aliases")]
    public bool? UseGitAliases { get; set; }
}