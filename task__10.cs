using System;
using System.Collections;
using System.Text;

/*
Переписан Класс BloomFilter. Убрана привязка значений, которые могут хранитсья в фильтре к типу String.
Он заменен интерфейсом IHashable, который позволяет сконвертировать объект в поток байт.
*/


public interface IHashable
{
    byte[] ToBytes();
}

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


    private static int RollingHash(int key, byte[] data, int filterLength)
    {
        long res = 0;
        foreach (byte b in data)
        {
            res = (res * key + b) % filterLength;
        }
        return (int)res;
    }

    private int Hash1(IHashable value) => RollingHash(17, value.ToBytes(), FilterLength);
    private int Hash2(IHashable value) => RollingHash(223, value.ToBytes(), FilterLength);


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


    public BloomFilter Add(IHashable value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        BitArray newBits = (BitArray)_bits.Clone();

        int p1 = Hash1(value);
        int p2 = Hash2(value);
        newBits[p1] = true;
        newBits[p2] = true;

        return new BloomFilter(FilterLength, newBits);
    }

    public bool IsValue(IHashable value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        int p1 = Hash1(value);
        int p2 = Hash2(value);
        return IsBitSet(p1) && IsBitSet(p2);
    }

}

public sealed class StringHashable : IHashable
{
    private readonly string _value;
    private readonly byte[] _bytes;
    public StringHashable(string value, Encoding? encoding = null)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        encoding ??= Encoding.UTF8;
        _bytes = encoding.GetBytes(_value);
    }
    public string get()
    {
        return _value;
    }

    public byte[] ToBytes() => _bytes;
}

class Program
{
    static void Main()
    {
        var bf = new BloomFilter(128);
        StringHashable present1 = new StringHashable("present_str1");
        StringHashable present2 = new StringHashable("present_str2");

        StringHashable absent1 = new StringHashable("absent_str");


        var bf1 = bf.Add(present1);
        var bf2 = bf1.Add(present2);

        Console.WriteLine($"\"{present1.get()}\" present in initial set?  {bf.IsValue(present1)}");
        Console.WriteLine($"\"{present2.get()}\" present in initial set?  {bf.IsValue(present1)}");
        Console.WriteLine($"\"{absent1.get()}\" present in initial set?  {bf.IsValue(present1)}");
        Console.WriteLine("--------------------");

        Console.WriteLine($"\"{present1.get()}\" present in second set?  {bf1.IsValue(present1)}");
        Console.WriteLine($"\"{present2.get()}\" present in second set?  {bf1.IsValue(present2)}");
        Console.WriteLine($"\"{absent1.get()}\" present in secon set?  {bf1.IsValue(absent1)}");
        Console.WriteLine("--------------------");

        Console.WriteLine($"\"{present1.get()}\" present in final set?  {bf2.IsValue(present1)}");
        Console.WriteLine($"\"{present2.get()}\" present in final set?  {bf2.IsValue(present2)}");
        Console.WriteLine($"\"{absent1.get()}\" present in final set?  {bf2.IsValue(absent1)}");
        Console.WriteLine("--------------------");

    }
}
