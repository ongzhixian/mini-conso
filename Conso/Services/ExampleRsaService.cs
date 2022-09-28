using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Conso.Services;

public class ExampleRsaService

{
    private readonly ILogger<ExampleRsaService> logger;

    public ExampleRsaService(ILogger<ExampleRsaService>? logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void DoWork()
    {
        try {
            //Create a UnicodeEncoder to convert between byte array and string.
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            //Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
            byte[] encryptedData;
            byte[] decryptedData;

            //Create a new instance of RSACryptoServiceProvider to generate
            //public and private key data.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider()) {

                //Pass the data to ENCRYPT, the public key information 
                //(using RSACryptoServiceProvider.ExportParameters(false),
                //and a boolean flag specifying no OAEP padding.
                encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), true);

                //Pass the data to DECRYPT, the private key information 
                //(using RSACryptoServiceProvider.ExportParameters(true),
                //and a boolean flag specifying no OAEP padding.
                decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), true);

                //Display the decrypted plaintext to the console. 
                Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
            }
        }
        catch (ArgumentNullException) {
            //Catch this exception in case the encryption did
            //not succeed.
            Console.WriteLine("Encryption failed.");
        }
    }


    public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
    {
        try {
            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider()) {

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                //RSA.ImportParameters(RSAKeyInfo);
                RSA.FromXmlString(@"<RSAKeyValue><Modulus>nynqKp7ayQ2fubvjdG2RnVN2NHwDVphSOeRV4h0d8vXhZLX3z7YfSfQYnDtkudqUr4ZJBnCnZuudZmCCX4hoGGDkC8DeA8GGi8wzMMOdyi8t/chYidgl3MX44xYdl2YslncAcUaRtrpVrY9/ZLc2EnPvI3xwSZUdLcjSc9myi46ZnfuZ87TRFHvhGyQIDvUaOfxrB/+5C3VutgNsUHFAbwsvCWFyMMXjXnxpLfkOONi+DVgf02mvblqRKWFqUDnjM2062RhlXRCg9dJKSsknyIF8/dPzCwW9aEG9IqdNWzpXfqIghwYiXt42PQhNcCiLboRGMvKJC4V3Dp4DUIVyQQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
        //Catch and display a CryptographicException  
        //to the console.
        catch (CryptographicException e) {
            Console.WriteLine(e.Message);

            return null;
        }
    }

    public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
    {
        try {
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider()) {
                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            return decryptedData;
        }
        //Catch and display a CryptographicException  
        //to the console.
        catch (CryptographicException e) {
            Console.WriteLine(e.ToString());

            return null;
        }
    }

}
