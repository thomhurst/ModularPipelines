namespace ModularPipelines.AmazonWebServices;

/// <summary>
/// Provides access to AWS resource provisioning services.
/// </summary>
public interface IAmazonProvisioner
{
    /// <summary>
    /// Gets the S3 service for bucket and object management.
    /// </summary>
    IS3 S3 { get; }
}