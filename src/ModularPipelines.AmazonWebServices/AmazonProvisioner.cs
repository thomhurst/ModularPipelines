namespace ModularPipelines.AmazonWebServices;

/// <summary>
/// Provides access to AWS resource provisioning services.
/// </summary>
internal class AmazonProvisioner : IAmazonProvisioner
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmazonProvisioner"/> class.
    /// </summary>
    /// <param name="s3">The S3 service instance.</param>
    public AmazonProvisioner(IS3 s3)
    {
        S3 = s3;
    }

    /// <inheritdoc />
    public IS3 S3 { get; }
}