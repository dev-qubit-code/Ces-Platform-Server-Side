namespace Ces_Platform_Server_Side.Validators.Constants;

public static class ConditionsConstants
{
    public static bool BeGenuineImage(IFormFile file)
    {
        
        using var stream = file.OpenReadStream();
        using var reader = new BinaryReader(stream);
        var headerBytes = reader.ReadBytes(12);
        
        // CRITICAL: Reset the stream back to 0 so Supabase uploads the full file!
        stream.Position = 0; 

        // If the file is completely empty or incredibly tiny, reject it
        if (headerBytes.Length < 4) return false;

        // 1. PNG Signature (89 50 4E 47)
        if (headerBytes[0] == 0x89 && headerBytes[1] == 0x50 && headerBytes[2] == 0x4E && headerBytes[3] == 0x47)
            return true;

        // 2. JPEG Signature (FF D8 FF)
        if (headerBytes[0] == 0xFF && headerBytes[1] == 0xD8 && headerBytes[2] == 0xFF)
            return true;

        // 3. WEBP Signature (RIFF .... WEBP)
        // Checks bytes 0-3 for "RIFF" and bytes 8-11 for "WEBP"
        if (headerBytes.Length >= 12 && 
            headerBytes[0] == 0x52 && headerBytes[1] == 0x49 && headerBytes[2] == 0x46 && headerBytes[3] == 0x46 &&
            headerBytes[8] == 0x57 && headerBytes[9] == 0x45 && headerBytes[10] == 0x42 && headerBytes[11] == 0x50)
            return true;

        // 4. GIF Signature (GIF8 - 47 49 46 38)
        if (headerBytes[0] == 0x47 && headerBytes[1] == 0x49 && headerBytes[2] == 0x46 && headerBytes[3] == 0x38)
            return true;

        // 5. BMP Signature (BM - 42 4D)
        if (headerBytes[0] == 0x42 && headerBytes[1] == 0x4D)
            return true;

        // 6. TIFF Signature (II* or MM* depending on byte order)
        if ((headerBytes[0] == 0x49 && headerBytes[1] == 0x49 && headerBytes[2] == 0x2A && headerBytes[3] == 0x00) ||
            (headerBytes[0] == 0x4D && headerBytes[1] == 0x4D && headerBytes[2] == 0x00 && headerBytes[3] == 0x2A))
            return true;

        // If it doesn't match any of these, it is NOT a trusted image format!
        return false;
    }
}