namespace ModularPipelines.AmazonWebServices;

public interface IAmazon
{
    IAmazonProvisioner Provisioner { get; }
}