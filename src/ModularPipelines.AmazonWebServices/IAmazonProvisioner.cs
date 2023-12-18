namespace ModularPipelines.AmazonWebServices;

public interface IAmazonProvisioner
{
    S3 S3 { get; }
}