using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PieceBackJigsaw
{

    static void ReadMD5AppendedFile(List<byte> bList, string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            bList.AddRange(fileBytes.Take(fileBytes.Length - 32));

        }
        else
        {
            Console.WriteLine($"[-] File '{filePath}' not found.");
        }
    }

    static void ReadAndAppendBytes(List<byte> bList, string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            bList.AddRange(fileBytes.Take(fileBytes.Length));

        }
        else
        {
            Console.WriteLine($"[-] File '{filePath}' not found.");
        }
    }

    static string GetNextFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                fileStream.Seek(-32, SeekOrigin.End);
                byte[] fileAsBytes = new byte[32];
                fileStream.Read(fileAsBytes, 0, 32);

                return Encoding.ASCII.GetString(fileAsBytes);
            }
        }
        else
        {
            Console.WriteLine($"[-] File '{filePath}' not found.");
            return string.Empty;
        }
    }

    public static void Main(string[] args)
    {
        List<byte> buffer = new List<byte>();
        int numFiles = 0;
        string file = "";

        string basePath = "C:\\Users\\lemmy\\Desktop\\temp\\";
        string lemmyz = "lemmy.z";

        string contents = File.ReadAllText(basePath + lemmyz);
        string[] parts = contents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2)
        {
            if (int.TryParse(parts[0], out int parsedInt))
            {
                numFiles = parsedInt;
                file = parts[1];
            }
            else
            {
                Console.WriteLine("[-] Failed to parse number of files");
            }

        }
        else
        {
            Console.WriteLine($"[-] {lemmyz} does not contain expected format");
        }

        for (int i = 0; i < numFiles - 1; i++)
        {

            ReadMD5AppendedFile(buffer, basePath + file);
            file = GetNextFile(basePath + file);
        }

        ReadAndAppendBytes(buffer, basePath + file);

        File.WriteAllBytes(basePath + "original", buffer.ToArray());
    }
}