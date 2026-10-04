using ModularPipelines.Secrets;

namespace ModularPipelines.Context;

/// <summary>
/// Provides security operations including certificates, cryptographic hashing, and runtime secret registration.
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

    /// <summary>
    /// Gets the pipeline's registry for masking secrets discovered at runtime.
    /// </summary>
    /// <remarks>
    /// Register each value before it reaches logs or command output. Registration affects
    /// subsequent output in this pipeline and does not redact output already written.
    /// </remarks>
    ISecretRegistry Secrets { get; }
}
