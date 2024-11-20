using System.Numerics;
using System.Text;

namespace MapCreationUsingSeed;
using System.Security.Cryptography;
public class Hasher
{
    private string stringSeed;
    public int seed;
    public Random random;
    
    public Hasher(string seed)
    {
        
        stringSeed = seed;
        this.seed =  GetHashedNumber(seed);
        random = new Random(this.seed);
        Console.WriteLine("hash: " +  this.seed);
    }
    static int GetHashedNumber(string input)
    {
        
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            int hashedValue = BitConverter.ToInt32(hashBytes, 0);
            return Math.Abs(hashedValue);
        }
    }
    static int HashWithSeed(string seed, string input)
    {
        string combined = seed + input;
        byte[] combinedBytes = Encoding.UTF8.GetBytes(combined);
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(combinedBytes);
            int hashedValue = BitConverter.ToInt32(hashBytes, 0);
            return Math.Abs(hashedValue);
        }
    }

    public int GetRandomFromCoords(Vector2 location)
    {
        string newValue = location.X.ToString() + location.Y;
        return HashWithSeed(stringSeed, newValue);
    }

    public bool GetRandomBool(int input)
    {
        return  (input & 0x80000000) != 0;
    }
    
    
}