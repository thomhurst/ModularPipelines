using ModularPipelines.Context;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to security operations including certificates and cryptographic hashing.
/// </summary>
internal class SecurityContext : ISecurityContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityContext"/> class.
    /// </summary>
    /// <param name="certificates">The certificates context for X.509 certificate operations.</param>
    /// <param name="hash">The hash context for cryptographic hashing operations.</param>
    public SecurityContext(ICertificatesContext certificates, IHashContext hash)
    {
        Certificates = certificates;
        Hash = hash;
    }

    /// <inheritdoc />
    public ICertificatesContext Certificates { get; }

    /// <inheritdoc />
    public IHashContext Hash { get; }
}
