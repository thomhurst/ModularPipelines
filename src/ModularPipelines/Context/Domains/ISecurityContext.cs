using ModularPipelines.Context.Domains.Security;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides security operations including certificates and cryptographic hashing.
/// </summary>
public interface ISecurityContext
{
    /// <summary>
    /// X.509 certificate operations.
    /// </summary>
    ICertificatesContext Certificates { get; }

    /// <summary>
    /// Cryptographic hashing for text and files.
    /// </summary>
    IHashContext Hash { get; }
}
