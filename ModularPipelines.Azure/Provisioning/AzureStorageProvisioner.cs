using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning;

public class AzureStorageProvisioner : BaseAzureProvisioner
{
    public AzureStorageProvisioner(ArmClient armClient) : base(armClient)
    {
    }
    
    public async Task<ArmOperation<StorageAccountResource>> StorageAccount(AzureResourceIdentifier azureResourceIdentifier, StorageAccountCreateOrUpdateContent properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetStorageAccounts()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
    
    public async Task<ArmOperation<BlobServiceResource>> BlobService(AzureResourceIdentifier azureResourceIdentifier, BlobServiceData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetBlobService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties);
    }
    
    public async Task<ArmOperation<BlobContainerResource>> BlobContainer(AzureResourceIdentifier azureResourceIdentifier, string containerName, BlobContainerData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetBlobService().GetBlobContainers().CreateOrUpdateAsync(WaitUntil.Completed, containerName, properties);
    }
    
    public async Task<ArmOperation<TableServiceResource>> TableService(AzureResourceIdentifier azureResourceIdentifier, TableServiceData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetTableService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties);
    }
    
    public async Task<ArmOperation<TableResource>> Table(AzureResourceIdentifier azureResourceIdentifier, string tableName, TableData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetTableService().GetTables().CreateOrUpdateAsync(WaitUntil.Completed, tableName, properties);
    }
    
    public async Task<ArmOperation<FileServiceResource>> FileService(AzureResourceIdentifier azureResourceIdentifier, FileServiceData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetFileService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties);
    }
    
    public async Task<ArmOperation<FileShareResource>> FileShare(AzureResourceIdentifier azureResourceIdentifier, string fileShareName, FileShareData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetFileService().GetFileShares().CreateOrUpdateAsync(WaitUntil.Completed, fileShareName, properties);
    }
    
    public async Task<ArmOperation<QueueServiceResource>> QueueService(AzureResourceIdentifier azureResourceIdentifier, QueueServiceData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetQueueService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties);
    }
    
    public async Task<ArmOperation<StorageQueueResource>> StorageQueue(AzureResourceIdentifier azureResourceIdentifier, string storageQueueName, StorageQueueData properties)
    {
        return await GetStorageAccount(azureResourceIdentifier).GetQueueService().GetStorageQueues().CreateOrUpdateAsync(WaitUntil.Completed, storageQueueName, properties);
    }

    private StorageAccountResource GetStorageAccount(AzureResourceIdentifier azureResourceIdentifier)
    {
        return GetResourceGroup(azureResourceIdentifier).GetStorageAccount(azureResourceIdentifier.ResourceName);
    }
}