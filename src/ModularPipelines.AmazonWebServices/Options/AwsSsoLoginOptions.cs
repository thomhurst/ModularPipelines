using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso", "login")]
public record AwsSsoLoginOptions : AwsOptions
{
    [CliFlag("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CliOption("--sso-session")]
    public string? SsoSession { get; set; }
}