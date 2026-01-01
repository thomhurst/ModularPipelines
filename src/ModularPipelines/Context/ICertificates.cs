using System.Security.Cryptography.X509Certificates;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for retrieving X.509 certificates from the certificate store.
/// </summary>
public interface ICertificates
{
    /// <summary>
    /// Gets a certificate by its subject name.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="subject">The subject name to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateBySubject(StoreLocation storeLocation, string subject);

    /// <summary>
    /// Gets a certificate by its thumbprint.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="thumbprint">The thumbprint to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateByThumbprint(StoreLocation storeLocation, string thumbprint);

    /// <summary>
    /// Gets a certificate by its serial number.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="serialNumber">The serial number to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateBySerialNumber(StoreLocation storeLocation, string serialNumber);

    /// <summary>
    /// Gets a certificate using a custom find type and value.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="findType">The type of search to perform.</param>
    /// <param name="findValue">The value to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateBy(StoreLocation storeLocation, X509FindType findType, string findValue);
}