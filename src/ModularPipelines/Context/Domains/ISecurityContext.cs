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
    /// Cryptographic hashing (SHA256, SHA512, MD5, etc.).
    /// </summary>
    IHasherContext Hasher { get; }
}
