using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "hl7v2-stores", "list")]
public record GcloudHealthcareHl7v2StoresListOptions(
[property: CliOption("--dataset")] string Dataset,
[property: CliOption("--location")] string Location
) : GcloudOptions;