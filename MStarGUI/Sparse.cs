// Decompiled with JetBrains decompiler
// Type: MStarBinToolGUI.Sparse
// Assembly: MStarBinToolGUI, Version=2.5.0.2, Culture=neutral, PublicKeyToken=null
// MVID: F90E615F-21EC-4258-B3C9-F0CFF9D3BC59
// Assembly location: C:\Anatoly\study\MStarBin\MStarBinTool-GUI_25\MStarBinToolGUI.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace MStarGUI
{
    public static class Sparse
    {
        public static void Compress (string src_File, string dst_File, long cnk_Size)
        {
            if (!File.Exists( src_File ))
                throw new Exception( "Не найден файл " + Path.GetFileName( src_File ) );
            if (cnk_Size < 4160L)
                throw new Exception( "Размер чанка слишком мал" );
            if (new FileInfo( src_File ).Length % 4096L > 0L)
                throw new Exception( "Исходный файл имеет некорректную длину" );
            using (FileStream fileStream1 = new FileStream( src_File, FileMode.Open, FileAccess.Read, FileShare.None, 65536, FileOptions.SequentialScan ))
            {
                int num = 0;
                while (true)
                {
                    using (FileStream fileStream2 = File.Open( string.Format( "{0}.chunk.{1}", (object)dst_File, (object)num ), FileMode.Create, FileAccess.ReadWrite, FileShare.None ))
                    {
                        fileStream2.SetLength( 0L );
                        if (Sparse.SparseCompressionHelper.WriteCompressedSparse( (Stream)fileStream1, (Stream)fileStream2, cnk_Size ))
                            break;
                    }
                    ++num;
                }
            }
        }

        public static void Decompress (List<string> src_CnkList, string dst_File)
        {
            using (FileStream fileStream1 = File.Open( dst_File, FileMode.Create, FileAccess.ReadWrite, FileShare.None ))
            {
                fileStream1.SetLength( 0L );
                foreach (string srcCnk in src_CnkList)
                {
                    using (FileStream fileStream2 = new FileStream( srcCnk, FileMode.Open, FileAccess.Read, FileShare.None, 65536, FileOptions.SequentialScan ))
                        Sparse.SparseDecompressionHelper.DecompressSparse( (Stream)fileStream2, (Stream)fileStream1 );
                }
            }
        }

        public enum ChunkType : ushort
        {
            Raw = 51905, // 0xCAC1
            Fill = 51906, // 0xCAC2
            DontCare = 51907, // 0xCAC3
            CRC = 51908, // 0xCAC4
        }

        public class LittleEndianConverter
        {
            public static ushort ToUInt16 (byte[] buffer, int offset)
            {
                return (ushort)((uint)buffer[offset + 1] << 8 | (uint)buffer[offset]);
            }

            public static uint ToUInt32 (byte[] buffer, int offset)
            {
                return (uint)((int)buffer[offset + 3] << 24 | (int)buffer[offset + 2] << 16 | (int)buffer[offset + 1] << 8) | (uint)buffer[offset];
            }

            public static byte[] GetBytes (ushort value)
            {
                return new byte[2]
                {
          (byte) ((uint) value & (uint) byte.MaxValue),
          (byte) ((int) value >> 8 & (int) byte.MaxValue)
                };
            }

            public static byte[] GetBytes (uint value)
            {
                return new byte[4]
                {
          (byte) (value & (uint) byte.MaxValue),
          (byte) (value >> 8 & (uint) byte.MaxValue),
          (byte) (value >> 16 & (uint) byte.MaxValue),
          (byte) (value >> 24 & (uint) byte.MaxValue)
                };
            }
        }

        public class LittleEndianWriter
        {
            public static void WriteUInt16 (byte[] buffer, int offset, ushort value)
            {
                byte[] bytes = Sparse.LittleEndianConverter.GetBytes( value );
                Array.Copy( (Array)bytes, 0, (Array)buffer, offset, bytes.Length );
            }

            public static void WriteUInt32 (byte[] buffer, int offset, uint value)
            {
                byte[] bytes = Sparse.LittleEndianConverter.GetBytes( value );
                Array.Copy( (Array)bytes, 0, (Array)buffer, offset, bytes.Length );
            }
        }

        public class ByteUtils
        {
            public static bool AreByteArraysEqual (byte[] array1, byte[] array2)
            {
                if (array1.Length != array2.Length)
                    return false;
                for (int index = 0; index < array1.Length; ++index)
                {
                    if ((int)array1[index] != (int)array2[index])
                        return false;
                }
                return true;
            }
        }

        public class ByteReader
        {
            public static byte[] ReadBytes (byte[] buffer, int offset, int length)
            {
                byte[] numArray = new byte[length];
                Array.Copy( (Array)buffer, offset, (Array)numArray, 0, length );
                return numArray;
            }
        }

        public class ByteWriter
        {
            public static void WriteBytes (byte[] buffer, int offset, byte[] bytes)
            {
                Sparse.ByteWriter.WriteBytes( buffer, offset, bytes, bytes.Length );
            }

            public static void WriteBytes (byte[] buffer, int offset, byte[] bytes, int length)
            {
                Array.Copy( (Array)bytes, 0, (Array)buffer, offset, length );
            }

            public static void WriteBytes (Stream stream, byte[] bytes)
            {
                stream.Write( bytes, 0, bytes.Length );
            }
        }

        public class SparseCompressionHelper
        {
            public const int BlockSize = 4096;

            public static bool WriteCompressedSparse (Stream input, Stream output, long maxSparseSize)
            {
                output.Seek( 28L, SeekOrigin.Begin );
                long num1 = 28;
                uint num2 = 0;
                if (input.Position != 0L)
                {
                    Sparse.SparseCompressionHelper.WriteDontCareChunk( output, (uint)((ulong)input.Position / 4096UL) );
                    ++num2;
                }
                MemoryStream memoryStream = new MemoryStream();
                byte[] numArray1 = (byte[])null;
                int num3 = 0;
                while (num1 + 12L + 4096L <= maxSparseSize && input.Position < input.Length)
                {
                    byte[] numArray2 = new byte[4096];
                    input.Read( numArray2, 0, 4096 );
                    byte[] compressBlock = Sparse.SparseCompressionHelper.TryToCompressBlock( numArray2 );
                    if (numArray1 != null)
                    {
                        if (compressBlock != null)
                        {
                            if (Sparse.ByteUtils.AreByteArraysEqual( numArray1, compressBlock ))
                            {
                                ++num3;
                            }
                            else
                            {
                                Sparse.SparseCompressionHelper.WriteFillChunk( output, numArray1, (uint)num3 );
                                ++num2;
                                num3 = 1;
                                numArray1 = compressBlock;
                                num1 += 16L;
                            }
                        }
                        else
                        {
                            Sparse.SparseCompressionHelper.WriteFillChunk( output, numArray1, (uint)num3 );
                            ++num2;
                            Sparse.ByteWriter.WriteBytes( (Stream)memoryStream, numArray2 );
                            numArray1 = (byte[])null;
                            num3 = 0;
                            num1 += 4108L;
                        }
                    }
                    else if (compressBlock != null)
                    {
                        Sparse.SparseCompressionHelper.WriteRawChunk( output, (Stream)memoryStream );
                        ++num2;
                        memoryStream = new MemoryStream();
                        num3 = 1;
                        numArray1 = compressBlock;
                        num1 += 16L;
                    }
                    else
                    {
                        Sparse.ByteWriter.WriteBytes( (Stream)memoryStream, numArray2 );
                        num1 += 4096L;
                    }
                }
                uint num4;
                if (memoryStream.Length > 0L)
                {
                    Sparse.SparseCompressionHelper.WriteRawChunk( output, (Stream)memoryStream );
                    num4 = num2 + 1U;
                }
                else
                {
                    Sparse.SparseCompressionHelper.WriteFillChunk( output, numArray1, (uint)num3 );
                    num4 = num2 + 1U;
                }
                int num5 = input.Position == input.Length ? 1 : 0;
                if (num5 == 0)
                {
                    Sparse.SparseCompressionHelper.WriteDontCareChunk( output, (uint)((ulong)(input.Length - input.Position) / 4096UL) );
                    ++num4;
                }
                output.Seek( 0L, SeekOrigin.Begin );
                new Sparse.SparseHeader()
                {
                    BlockSize = 4096U,
                    TotalBlocks = ((uint)((ulong)input.Length / 4096UL)),
                    TotalChunks = num4
                }.WriteBytes( output );
                output.Close();
                return num5 != 0;
            }

            public static byte[] TryToCompressBlock (byte[] block)
            {
                if (block.Length % 4 > 0)
                    throw new ArgumentException( "Некорректный размер блока" );
                byte[] numArray = Sparse.ByteReader.ReadBytes( block, 0, 4 );
                int index = 4;
                while (index < block.Length)
                {
                    if ((int)numArray[0] != (int)block[index] || (int)numArray[1] != (int)block[index + 1] || ((int)numArray[2] != (int)block[index + 2] || (int)numArray[3] != (int)block[index + 3]))
                        return (byte[])null;
                    index += 4;
                }
                return numArray;
            }

            public static void WriteFillChunk (Stream output, byte[] fill, uint blockCount)
            {
                new Sparse.ChunkHeader()
                {
                    ChunkType = Sparse.ChunkType.Fill,
                    ChunkSize = blockCount,
                    TotalSize = 16U
                }.WriteBytes( output );
                Sparse.ByteWriter.WriteBytes( output, fill );
            }

            public static void WriteRawChunk (Stream output, Stream rawChunk)
            {
                new Sparse.ChunkHeader()
                {
                    ChunkType = Sparse.ChunkType.Raw,
                    ChunkSize = ((uint)((ulong)rawChunk.Length / 4096UL)),
                    TotalSize = (12U + (uint)rawChunk.Length)
                }.WriteBytes( output );
                rawChunk.Seek( 0L, SeekOrigin.Begin );
                long num = rawChunk.Length / 4096L;
                for (long index = 0; index < num; ++index)
                {
                    byte[] numArray = new byte[4096];
                    rawChunk.Read( numArray, 0, 4096 );
                    Sparse.ByteWriter.WriteBytes( output, numArray );
                }
            }

            public static void WriteDontCareChunk (Stream output, uint blockLength)
            {
                new Sparse.ChunkHeader()
                {
                    ChunkType = Sparse.ChunkType.DontCare,
                    ChunkSize = blockLength,
                    TotalSize = 12U
                }.WriteBytes( output );
            }
        }

        public class SparseDecompressionHelper
        {
            public static void DecompressSparse (Stream input, Stream output)
            {
                Sparse.SparseHeader sparseHeader = Sparse.SparseHeader.Read( input );
                if (sparseHeader == null)
                    throw new ArgumentException( "Некорректный формат исходного файла" );
                output.Seek( 0L, SeekOrigin.Begin );
                for (uint index = 0; index < sparseHeader.TotalChunks; ++index)
                {
                    Sparse.ChunkHeader chunkHeader = Sparse.ChunkHeader.Read( input );
                    long offset1 = (long)chunkHeader.ChunkSize * (long)sparseHeader.BlockSize;
                    switch (chunkHeader.ChunkType)
                    {
                        case Sparse.ChunkType.Raw:
                            byte[] bytes1 = Sparse.SparseDecompressionHelper.ReadBytes( input, (int)offset1 );
                            Sparse.ByteWriter.WriteBytes( output, bytes1 );
                            break;
                        case Sparse.ChunkType.Fill:
                            byte[] bytes2 = Sparse.SparseDecompressionHelper.ReadBytes( input, 4 );
                            byte[] numArray = new byte[offset1];
                            int offset2 = 0;
                            while (offset2 < numArray.Length)
                            {
                                Sparse.ByteWriter.WriteBytes( numArray, offset2, bytes2 );
                                offset2 += 4;
                            }
                            Sparse.ByteWriter.WriteBytes( output, numArray );
                            break;
                        case Sparse.ChunkType.DontCare:
                            output.Seek( offset1, SeekOrigin.Current );
                            break;
                        case Sparse.ChunkType.CRC:
                            Sparse.SparseDecompressionHelper.ReadBytes( input, 4 );
                            break;
                        default:
                            throw new ArgumentException( "Некоррктный тип чанка" );
                    }
                }
                input.Close();
            }

            public static byte[] ReadBytes (Stream stream, int length)
            {
                byte[] buffer = new byte[length];
                stream.Read( buffer, 0, length );
                return buffer;
            }
        }

        public class ChunkHeader
        {
            public const int Length = 12;
            public Sparse.ChunkType ChunkType;
            public ushort Reserved;
            public uint ChunkSize;
            public uint TotalSize;

            public ChunkHeader ()
            {
            }

            public ChunkHeader (byte[] buffer, int offset)
            {
                this.ChunkType = (Sparse.ChunkType)Sparse.LittleEndianConverter.ToUInt16( buffer, offset );
                this.Reserved = Sparse.LittleEndianConverter.ToUInt16( buffer, offset + 2 );
                this.ChunkSize = Sparse.LittleEndianConverter.ToUInt32( buffer, offset + 4 );
                this.TotalSize = Sparse.LittleEndianConverter.ToUInt32( buffer, offset + 8 );
            }

            public void WriteBytes (byte[] buffer, int offset)
            {
                Sparse.LittleEndianWriter.WriteUInt16( buffer, offset, (ushort)this.ChunkType );
                Sparse.LittleEndianWriter.WriteUInt16( buffer, offset + 2, this.Reserved );
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset + 4, this.ChunkSize );
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset + 8, this.TotalSize );
            }

            public void WriteBytes (Stream stream)
            {
                byte[] numArray = new byte[12];
                this.WriteBytes( numArray, 0 );
                Sparse.ByteWriter.WriteBytes( stream, numArray );
            }

            public static Sparse.ChunkHeader Read (Stream stream)
            {
                byte[] buffer = new byte[12];
                stream.Read( buffer, 0, 12 );
                return new Sparse.ChunkHeader( buffer, 0 );
            }
        }

        public class SparseHeader
        {
            public const uint ValidSignature = 3978755898;
            public const int Length = 28;
            public uint Magic;
            public ushort MajorVersion;
            public ushort MinorVersion;
            public ushort FileHeaderSize;
            public ushort ChunkHeaderSize;
            public uint BlockSize;
            public uint TotalBlocks;
            public uint TotalChunks;
            public uint ImageChecksum;

            public SparseHeader ()
            {
                this.Magic = 3978755898U;
                this.MajorVersion = (ushort)1;
                this.MinorVersion = (ushort)0;
                this.FileHeaderSize = (ushort)28;
                this.ChunkHeaderSize = (ushort)12;
            }

            public SparseHeader (byte[] buffer, int offset)
            {
                this.Magic = Sparse.LittleEndianConverter.ToUInt32( buffer, offset );
                this.MajorVersion = Sparse.LittleEndianConverter.ToUInt16( buffer, offset + 4 );
                this.MinorVersion = Sparse.LittleEndianConverter.ToUInt16( buffer, offset + 6 );
                this.FileHeaderSize = Sparse.LittleEndianConverter.ToUInt16( buffer, offset + 8 );
                this.ChunkHeaderSize = Sparse.LittleEndianConverter.ToUInt16( buffer, offset + 10 );
                this.BlockSize = Sparse.LittleEndianConverter.ToUInt32( buffer, offset + 12 );
                this.TotalBlocks = Sparse.LittleEndianConverter.ToUInt32( buffer, offset + 16 );
                this.TotalChunks = Sparse.LittleEndianConverter.ToUInt32( buffer, offset + 20 );
                this.ImageChecksum = Sparse.LittleEndianConverter.ToUInt32( buffer, offset + 24 );
            }

            public void WriteBytes (byte[] buffer, int offset)
            {
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset, this.Magic );
                Sparse.LittleEndianWriter.WriteUInt16( buffer, offset + 4, this.MajorVersion );
                Sparse.LittleEndianWriter.WriteUInt16( buffer, offset + 6, this.MinorVersion );
                Sparse.LittleEndianWriter.WriteUInt16( buffer, offset + 8, this.FileHeaderSize );
                Sparse.LittleEndianWriter.WriteUInt16( buffer, offset + 10, this.ChunkHeaderSize );
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset + 12, this.BlockSize );
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset + 16, this.TotalBlocks );
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset + 20, this.TotalChunks );
                Sparse.LittleEndianWriter.WriteUInt32( buffer, offset + 24, this.ImageChecksum );
            }

            public void WriteBytes (Stream stream)
            {
                byte[] numArray = new byte[28];
                this.WriteBytes( numArray, 0 );
                Sparse.ByteWriter.WriteBytes( stream, numArray );
            }

            public static Sparse.SparseHeader Read (byte[] buffer, int offset)
            {
                if ((int)Sparse.LittleEndianConverter.ToUInt32( buffer, offset ) == -316211398)
                    return new Sparse.SparseHeader( buffer, offset );
                return (Sparse.SparseHeader)null;
            }

            public static Sparse.SparseHeader Read (Stream stream)
            {
                byte[] buffer = new byte[28];
                stream.Read( buffer, 0, 28 );
                return Sparse.SparseHeader.Read( buffer, 0 );
            }
        }
    }
}
