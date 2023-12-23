using ModularPipelines.AmazonWebServices.Services;

namespace ModularPipelines.AmazonWebServices;

internal class Amazon : IAmazon
{
    public Amazon(IAmazonProvisioner provisioner, Aws awsCli)
    {
        Provisioner = provisioner;
        AwsCli = awsCli;
    }

    public IAmazonProvisioner Provisioner { get; }
    public Aws AwsCli { get; }
}