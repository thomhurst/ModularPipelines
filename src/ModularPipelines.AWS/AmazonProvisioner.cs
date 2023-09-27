namespace ModularPipelines.AWS;

internal class AmazonProvisioner : IAmazonProvisioner
{
    public AmazonProvisioner(S3 s3)
    {
        S3 = s3;
    }

    public S3 S3 { get; }
}