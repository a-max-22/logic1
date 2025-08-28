using System;
using System.Collections;


public class BloomFilter
{
    public int FilterLength { get; }

    private readonly BitArray _bits;
    private const int MaskUnitSize = 32;

    public BloomFilter(int filterLength)
    {
        if (filterLength <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(filterLength), "Filter length must be positive.");

        FilterLength = filterLength;
        _bits = new BitArray(filterLength);
    }

    private BloomFilter(int filterLength, BitArray bits)
    {
        FilterLength = filterLength;
        _bits = bits;
    }


    private int GeneralHash(int key, string value)
    {
        long res = 0;
        foreach (char c in value)
        {
            int code = c;
            res = (res * key + code) % FilterLength;
        }
        return (int)res;
    }

    private int Hash1(string s) => GeneralHash(17, s); 
    private int Hash2(string s) => GeneralHash(223, s);


    private void ValidatePosition(int pos)
    {
        if (pos < 0 || pos >= FilterLength)
            throw new ArgumentOutOfRangeException(
                nameof(pos),
                $"Position {pos} is outside of bit‑field bounds (0 … {FilterLength - 1}).");
    }

    private void SetBit(int pos)
    {
        ValidatePosition(pos);
        _bits[pos] = true;
    }

    private bool IsBitSet(int pos)
    {
        ValidatePosition(pos);
        return _bits[pos];
    }


    public BloomFilter Add(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        BitArray newBits = (BitArray)_bits.Clone();

        int p1 = Hash1(value);
        int p2 = Hash2(value);
        newBits[p1] = true;
        newBits[p2] = true;

        return new BloomFilter(FilterLength, newBits);
    }

    public bool IsValue(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        int p1 = Hash1(value);
        int p2 = Hash2(value);
        return IsBitSet(p1) && IsBitSet(p2);
    }

}

class Program
{
    static void Main()
    {
        var bf = new BloomFilter(128);
        string present1 = "present_str1";
        string present2 = "present_str2";

        string absent1 = "absent_str";


        var bf1 = bf.Add(present1);
        var bf2 = bf1.Add(present2);

        Console.WriteLine($"\"{present1}\" present in initial set?  {bf.IsValue(present1)}");
        Console.WriteLine($"\"{present2}\" present in initial set?  {bf.IsValue(present1)}");
        Console.WriteLine($"\"{absent1}\" present in initial set?  {bf.IsValue(present1)}");
        Console.WriteLine("--------------------");

        Console.WriteLine($"\"{present1}\" present in second set?  {bf1.IsValue(present1)}");
        Console.WriteLine($"\"{present2}\" present in second set?  {bf1.IsValue(present2)}");
        Console.WriteLine($"\"{absent1}\" present in secon set?  {bf1.IsValue(absent1)}");
        Console.WriteLine("--------------------");

        Console.WriteLine($"\"{present1}\" present in final set?  {bf2.IsValue(present1)}");
        Console.WriteLine($"\"{present2}\" present in final set?  {bf2.IsValue(present2)}");
        Console.WriteLine($"\"{absent1}\" present in final set?  {bf2.IsValue(absent1)}");
        Console.WriteLine("--------------------");

    }
}
