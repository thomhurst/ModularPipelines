using ModularPipelines.Context;
using ModularPipelines.Secrets;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to security operations including certificates, cryptographic hashing, and runtime secret registration.
/// </summary>
internal class SecurityContext : ISecurityContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityContext"/> class.
    /// </summary>
    /// <param name="certificates">The certificates context for X.509 certificate operations.</param>
    /// <param name="hash">The hash context for cryptographic hashing operations.</param>
    /// <param name="secrets">The pipeline secret registry.</param>
    public SecurityContext(ICertificatesContext certificates, IHashContext hash, ISecretRegistry secrets)
    {
        Certificates = certificates;
        Hash = hash;
        Secrets = secrets;
    }

    /// <inheritdoc />
    public ICertificatesContext Certificates { get; }

    /// <inheritdoc />
    public IHashContext Hash { get; }

    /// <inheritdoc />
    public ISecretRegistry Secrets { get; }
}
