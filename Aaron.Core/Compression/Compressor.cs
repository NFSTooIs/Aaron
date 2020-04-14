﻿using System;
using System.Runtime.InteropServices;

namespace Aaron.Core.Compression
{
    public static class Compressor
    {
        public static int Decompress(byte[] compressedData, byte[] decompressedData)
        {
            return _internal_decompress(compressedData, compressedData.Length, decompressedData,
                decompressedData.Length);
        }

        public static int Compress(byte[] uncompressedData, ref byte[] compressedData)
        {
            var size = _internal_compress(uncompressedData, uncompressedData.Length, compressedData);

            if (compressedData.Length < size)
            {
                throw new CompressorException(
                    $"Compression output buffer was too small for the compressor! Wanted {size} bytes but the buffer was {compressedData.Length} bytes");
            }

            Array.Resize(ref compressedData, size);

            return size;
        }

        // unsigned char* in, int in_size, unsigned char* out, int out_size
        [DllImport("NativeLibrary", EntryPoint = "LZDecompress", CallingConvention = CallingConvention.Cdecl)]
        private static extern int _internal_decompress([In] byte[] inData, int inSize, [Out] byte[] outData,
            int outSize);

        // unsigned char* in, int in_size, unsigned char* out
        [DllImport("NativeLibrary", EntryPoint = "JLZCompress", CallingConvention = CallingConvention.Cdecl)]
        private static extern int _internal_compress([In] byte[] inData, int inSize, [Out] byte[] outData);
    }
}