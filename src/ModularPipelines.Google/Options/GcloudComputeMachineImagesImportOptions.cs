using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "import")]
public record GcloudComputeMachineImagesImportOptions(
[property: CliArgument] string Image,
[property: CliOption("--source-uri")] string SourceUri
) : GcloudOptions
{
    [CliFlag("--no-address")]
    public bool? NoAddress { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--byol")]
    public bool? Byol { get; set; }

    [CliFlag("--can-ip-forward")]
    public bool? CanIpForward { get; set; }

    [CliOption("--cloudbuild-service-account")]
    public string? CloudbuildServiceAccount { get; set; }

    [CliOption("--compute-service-account")]
    public string? ComputeServiceAccount { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--guest-environment")]
    public bool? GuestEnvironment { get; set; }

    [CliFlag("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [CliOption("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--log-location")]
    public string? LogLocation { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-tier")]
    public string? NetworkTier { get; set; }

    [CliOption("--os")]
    public string? Os { get; set; }

    [CliFlag("--restart-on-failure")]
    public bool? RestartOnFailure { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CliOption("--custom-memory")]
    public string? CustomMemory { get; set; }

    [CliFlag("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CliOption("--custom-vm-type")]
    public string? CustomVmType { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliFlag("--no-scopes")]
    public bool? NoScopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliFlag("--no-service-account")]
    public bool? NoServiceAccount { get; set; }
}