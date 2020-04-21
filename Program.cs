using Medallion.Shell;
using System;
using System.IO;

namespace Cert_Generator_Mqtt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start processing certificates!");
            
            var files = Directory.GetFiles(args[0]);
            var processedCertificatesFolder = string.Concat(args[0], @"\NewFiles");
            if(!Directory.Exists(processedCertificatesFolder))
            {
                Directory.CreateDirectory(processedCertificatesFolder);
            }

            foreach (var filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                var certificateFile = string.Empty;
                if(fileName.Contains(".pem.crt"))
                {
                    certificateFile = Path.Combine(processedCertificatesFolder, "certificate.cert.pem");                    
                }
                else if(fileName.Contains("private.pem"))
                {
                    certificateFile = Path.Combine(processedCertificatesFolder, "certificate.private.key");
                }
                else if(fileName.Contains("public.pem"))
                {
                    certificateFile = Path.Combine(processedCertificatesFolder, "certificate.public.key");
                }
                else if(fileName.Equals("AmazonRootCA1.crt"))
                {
                    certificateFile = Path.Combine(processedCertificatesFolder, fileName);
                }
                if(!Path.GetExtension(fileName).Equals(".sh"))
                {
                    File.Copy(filePath, certificateFile);
                }                
            }

            var command = Command.Run(@"C:\Program Files\Git\bin\sh.exe", @"E:\SafeFleet-ABLE\Docs\certificates\ConvertDeviceCertificate-To-PFX.sh");
            command.Wait();
        }
    }
}
/*
 * - Device certificate - This file usually ends with ".pem.crt". When you download this it will save as .txt file extension in windows. 
 * Save it in your certificates directory as 'certificates\certificate.cert.pem' and make sure that it is of file type '.pem', not 'txt' or '.crt'

- Device public key - This file usually ends with ".pem" and is of file type ".key". Save this file as 'certificates\certificate.public.key'.

- Device private key - This file usually ends with ".pem" and is of file type ".key". Save this file as 'certificates\certificate.private.key'. Make sure that this file is referred with suffix ".key" in the code while making MQTT connection to AWS IoT.

- Root certificate - Download from https://www.amazontrust.com/repository/AmazonRootCA1.pem. Save this file to 'certificates\AmazonRootCA1.crt'
 */
