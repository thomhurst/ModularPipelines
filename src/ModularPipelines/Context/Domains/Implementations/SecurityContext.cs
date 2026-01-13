using ModularPipelines.Context.Domains.Security;

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
    /// <param name="hasher">The hasher context for cryptographic hashing operations.</param>
    public SecurityContext(ICertificatesContext certificates, IHasherContext hasher)
    {
        Certificates = certificates;
        Hasher = hasher;
    }

    /// <inheritdoc />
    public ICertificatesContext Certificates { get; }

    /// <inheritdoc />
    public IHasherContext Hasher { get; }
}
