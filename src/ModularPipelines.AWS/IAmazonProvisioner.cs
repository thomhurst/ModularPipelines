namespace ModularPipelines.AWS;

public interface IAmazonProvisioner
{
    S3 S3 { get; }
}