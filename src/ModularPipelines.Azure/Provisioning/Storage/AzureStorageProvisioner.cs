using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Storage;

public class AzureStorageProvisioner : BaseAzureProvisioner
{
    public AzureStorageProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<StorageAccountResource>> StorageAccountAsync(AzureResourceIdentifier azureResourceIdentifier, StorageAccountCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetStorageAccounts()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<BlobServiceResource>> BlobServiceAsync(AzureResourceIdentifier azureResourceIdentifier, BlobServiceData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetBlobService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties, cancellationToken);
    }

    public async Task<ArmOperation<BlobContainerResource>> BlobContainerAsync(AzureResourceIdentifier azureResourceIdentifier, string containerName, BlobContainerData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetBlobService().GetBlobContainers().CreateOrUpdateAsync(WaitUntil.Completed, containerName, properties, cancellationToken);
    }

    public async Task<ArmOperation<TableServiceResource>> TableServiceAsync(AzureResourceIdentifier azureResourceIdentifier, TableServiceData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetTableService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties, cancellationToken);
    }

    public async Task<ArmOperation<TableResource>> TableAsync(AzureResourceIdentifier azureResourceIdentifier, string tableName, TableData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetTableService().GetTables().CreateOrUpdateAsync(WaitUntil.Completed, tableName, properties, cancellationToken);
    }

    public async Task<ArmOperation<FileServiceResource>> FileServiceAsync(AzureResourceIdentifier azureResourceIdentifier, FileServiceData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetFileService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties, cancellationToken);
    }

    public async Task<ArmOperation<FileShareResource>> FileShareAsync(AzureResourceIdentifier azureResourceIdentifier, string fileShareName, FileShareData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileShareName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetFileService().GetFileShares().CreateOrUpdateAsync(WaitUntil.Completed, fileShareName, properties, expand: null, cancellationToken);
    }

    public async Task<ArmOperation<QueueServiceResource>> QueueServiceAsync(AzureResourceIdentifier azureResourceIdentifier, QueueServiceData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetQueueService()
            .CreateOrUpdateAsync(WaitUntil.Completed, properties, cancellationToken);
    }

    public async Task<ArmOperation<StorageQueueResource>> StorageQueueAsync(AzureResourceIdentifier azureResourceIdentifier, string storageQueueName, StorageQueueData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(storageQueueName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetStorageAccount(azureResourceIdentifier).GetQueueService().GetStorageQueues().CreateOrUpdateAsync(WaitUntil.Completed, storageQueueName, properties, cancellationToken);
    }

    private StorageAccountResource GetStorageAccount(AzureResourceIdentifier azureResourceIdentifier)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return GetResourceGroup(azureResourceIdentifier).GetStorageAccount(azureResourceIdentifier.ResourceName);
    }
}
