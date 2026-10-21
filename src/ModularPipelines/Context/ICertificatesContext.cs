using System.Security.Cryptography.X509Certificates;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for retrieving X.509 certificates from the certificate store.
/// </summary>
/// <remarks>The caller must dispose every certificate returned by these methods.</remarks>
public interface ICertificatesContext
{
    /// <summary>
    /// Gets a certificate from the personal store using a case-insensitive subject substring match, including invalid certificates.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="subject">The subject name to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateBySubject(StoreLocation storeLocation, string subject);

    /// <summary>
    /// Gets a certificate by its subject from the specified store.
    /// </summary>
    /// <param name="storeName">The store to search.</param>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="subject">The full distinguished name when <paramref name="exactMatch"/> is true; otherwise a subject substring.</param>
    /// <param name="validOnly">Whether to require a valid certificate chain, including trust and validity periods.</param>
    /// <param name="exactMatch">Whether to match the full distinguished name instead of a subject substring, ignoring case.</param>
    /// <returns>The first matching certificate, or null if not found. The caller must dispose it.</returns>
    X509Certificate2? GetCertificateBySubject(StoreName storeName, StoreLocation storeLocation, string subject, bool validOnly = true, bool exactMatch = true);

    /// <summary>
    /// Gets a certificate by its thumbprint from the personal store, including invalid certificates.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="thumbprint">The thumbprint to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateByThumbprint(StoreLocation storeLocation, string thumbprint);

    /// <summary>
    /// Gets a certificate by its thumbprint from the specified store.
    /// </summary>
    /// <param name="storeName">The store to search.</param>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="thumbprint">The thumbprint to search for.</param>
    /// <param name="validOnly">Whether to require a valid certificate chain, including trust and validity periods.</param>
    /// <returns>The first matching certificate, or null if not found. The caller must dispose it.</returns>
    X509Certificate2? GetCertificateByThumbprint(StoreName storeName, StoreLocation storeLocation, string thumbprint, bool validOnly = true);

    /// <summary>
    /// Gets a certificate by its serial number from the personal store, including invalid certificates.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="serialNumber">The serial number to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateBySerialNumber(StoreLocation storeLocation, string serialNumber);

    /// <summary>
    /// Gets a certificate by its serial number from the specified store.
    /// </summary>
    /// <param name="storeName">The store to search.</param>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="serialNumber">The serial number to search for.</param>
    /// <param name="validOnly">Whether to require a valid certificate chain, including trust and validity periods.</param>
    /// <returns>The first matching certificate, or null if not found. The caller must dispose it.</returns>
    X509Certificate2? GetCertificateBySerialNumber(StoreName storeName, StoreLocation storeLocation, string serialNumber, bool validOnly = true);

    /// <summary>
    /// Gets a certificate from the personal store using a custom find type and value, including invalid certificates.
    /// </summary>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="findType">The type of search to perform.</param>
    /// <param name="findValue">The value to search for.</param>
    /// <returns>The matching certificate, or null if not found.</returns>
    X509Certificate2? GetCertificateBy(StoreLocation storeLocation, X509FindType findType, string findValue);

    /// <summary>
    /// Gets a certificate from the specified store using a custom find type and value.
    /// </summary>
    /// <param name="storeName">The store to search.</param>
    /// <param name="storeLocation">The store location to search.</param>
    /// <param name="findType">The type of search to perform.</param>
    /// <param name="findValue">The value to search for.</param>
    /// <param name="validOnly">Whether to require a valid certificate chain, including trust and validity periods.</param>
    /// <returns>The first matching certificate, or null if not found. The caller must dispose it.</returns>
    X509Certificate2? GetCertificateBy(StoreName storeName, StoreLocation storeLocation, X509FindType findType, string findValue, bool validOnly = true);
}
