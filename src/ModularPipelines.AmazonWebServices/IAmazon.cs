using ModularPipelines.AmazonWebServices.Services;

namespace ModularPipelines.AmazonWebServices;

public interface IAmazon
{
    IAmazonProvisioner Provisioner { get; }
    Aws AwsCli { get; }
}