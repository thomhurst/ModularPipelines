using ModularPipelines.AmazonWebServices.Services;

namespace ModularPipelines.AmazonWebServices;

internal class Amazon : IAmazon
{
    public Amazon(IAmazonProvisioner provisioner, IAws awsCli)
    {
        Provisioner = provisioner;
        AwsCli = awsCli;
    }

    public IAmazonProvisioner Provisioner { get; }

    public IAws AwsCli { get; }
}
