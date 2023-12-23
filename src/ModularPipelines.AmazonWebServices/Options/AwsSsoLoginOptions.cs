using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso", "login")]
public record AwsSsoLoginOptions : AwsOptions
{
    [BooleanCommandSwitch("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CommandSwitch("--sso-session")]
    public string? SsoSession { get; set; }
}