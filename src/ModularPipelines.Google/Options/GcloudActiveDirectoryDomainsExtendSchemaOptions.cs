using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "extend-schema")]
public record GcloudActiveDirectoryDomainsExtendSchemaOptions(
[property: CliArgument] string Domain,
[property: CliOption("--description")] string Description,
[property: CliOption("--ldif-file")] string LdifFile
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}