using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "users", "delete")]
public record GcloudAlloydbUsersDeleteOptions(
[property: CliArgument] string Username,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions;