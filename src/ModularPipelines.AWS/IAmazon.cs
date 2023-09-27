namespace ModularPipelines.AWS;

public interface IAmazon
{
    IAmazonProvisioner Provisioner { get; }
}