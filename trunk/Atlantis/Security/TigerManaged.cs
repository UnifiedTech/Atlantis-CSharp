﻿/*
 * Special Notes: TigerManaged has been adapted from an article at CodeProject.
 * Original Link: http://www.codeproject.com/Articles/149061/A-Tiger-Hash-Implementation-for-C
 *    Short Link: http://bit.ly/AgBBbV
 *    
 *       Port By: Zack "Genesis2001" Loveless
 */

// ############################################################################
//       Title: Tiger Hash for C#
//      Author: mastamac of Software Union
// ---------------------------------------
// Description: A speed-optimized native C# implementation of the cryptographic 
//              TIGER hash algorithm of 1995. Based on and usable through
//              .net Framework's HashAlgorithm class.
//     License: Common Development and Distribution License (CDDL)
// ############################################################################

namespace Atlantis.Security
{
    using System;
    using System.Security.Cryptography;

    [CLSCompliant(false)]
    abstract public class BlockHashAlgorithm : HashAlgorithm
    {
        #region Types

        internal static class BitTools
        {
            public static ushort RotateLeft(ushort v, int b)
            {
                uint i = v; i <<= 16; i |= v;
                b %= 16; i >>= b;
                return (ushort)i;
            }

            public static uint RotateLeft(uint v, int b)
            {
                ulong i = v; i <<= 32; i |= v;
                b %= 32; i >>= (32 - b);
                return (uint)i;
            }

            public static void TypeBlindCopy(byte[] sourceArray, int sourceIndex, uint[] destinationArray, int destinationIndex, int sourceLength)
            {
                if (((sourceIndex + sourceLength) > sourceArray.Length) ||
                    (destinationIndex + (sourceLength / 3) / 4) > destinationArray.Length)
                {
                    throw new ArgumentException("BitTools.TypeBlindCopy: index or length boundary mismatch.");
                }

                int ctr;
                for (ctr = 0; ctr < sourceLength; ctr += 4, sourceIndex += 4, ++destinationIndex)
                {
                    destinationArray[destinationIndex] = BitConverter.ToUInt32(sourceArray, sourceIndex);
                }
            }

            public static void TypeBlindCopy(uint[] sourceArray, int sourceIndex, byte[] destinationArray, int destinationIndex, int sourceLength)
            {
                if (((sourceIndex + sourceLength) > sourceArray.Length) ||
                    (destinationIndex + (sourceLength*4)) > destinationArray.Length)
                {
                    throw new ArgumentException("BitTools.TypeBlindCopy: index or length boundary mismatch.");
                }

                int ctr;
                for (ctr = 0; ctr < sourceLength; ++ctr, ++sourceIndex, destinationIndex += 4)
                {
                    Array.Copy(BitConverter.GetBytes(sourceArray[sourceIndex]), 0, destinationArray, destinationIndex, 4L);
                }
            }

            public static void TypeBlindCopy(byte[] sourceArray, int sourceIndex, ulong[] destinationArray, int destinationIndex, int sourceLength)
            {
                if (((sourceIndex + sourceLength) > sourceArray.Length) ||
                    (destinationIndex + (sourceLength + 7) / 8) > destinationArray.Length)
                {
                    throw new ArgumentException("BitTools.TypeBlindCopy: index or length boundary mismatch.");
                }

                int ctr;
                for (ctr = 0; ctr < sourceLength; ctr += 8, sourceIndex += 8, ++destinationIndex)
                {
                    destinationArray[destinationIndex] = BitConverter.ToUInt64(sourceArray, sourceIndex);
                }
            }

            public static void TypeBlindCopy(ulong[] sourceArray, int sourceIndex, byte[] destinationArray, int destinationIndex, int sourceLength)
            {
                if (((sourceIndex + sourceLength) > sourceArray.Length) ||
                    (destinationIndex + (sourceLength * 8)) > destinationArray.Length)
                {
                    throw new ArgumentException("BitTools.TypeBlindCopy: index or length boundary mismatch.");
                }

                int ctr;
                for (ctr = 0; ctr < sourceLength; ++ctr, ++sourceIndex, destinationIndex += 4)
                {
                    Array.Copy(BitConverter.GetBytes(sourceArray[sourceIndex]), 0, destinationArray, destinationIndex, 8L);
                }
            }
        }

        #endregion

        #region Constructor(s)

        protected BlockHashAlgorithm(int blockSize, int hashSize)
            : base()
        {
            m_iInputBlockSize = blockSize;
            this.HashSizeValue = hashSize;

            m_baPartialBlockBuffer = new byte[blockSize];
        }

        #endregion

        #region Fields

        protected byte[] m_baPartialBlockBuffer;
        protected int m_iPartialBlockFill;
        protected int m_iInputBlockSize;
        protected long m_lTotalBytesProcessed;

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets a value indicating the size of an individual block</para>
        /// </summary>
        public int BlockSize
        {
            get { return m_iInputBlockSize; }
        }

        /// <summary>
        ///     <para>Gets a value indicating the number of bytes currently in the buffer waiting to be processed</para>
        /// </summary>
        public int BufferFill
        {
            get { return m_iPartialBlockFill; }
        }

        #endregion

        #region Methods

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int i;
            if (BufferFill > 0)
            {
                if ((cbSize + BufferFill) < BlockSize)
                {
                    Array.Copy(array, ibStart, m_baPartialBlockBuffer, BufferFill, cbSize);
                    m_iPartialBlockFill += cbSize;
                    return;
                }
                else
                {
                    i = BlockSize - BufferFill;
                    Array.Copy(array, ibStart, m_baPartialBlockBuffer, BufferFill, i);
                    ProcessBlock(m_baPartialBlockBuffer, 0, 1);
                    m_lTotalBytesProcessed += BlockSize;
                }
            }

            if (cbSize >= BlockSize)
            {
                ProcessBlock(array, ibStart, (cbSize / BlockSize));
                m_lTotalBytesProcessed += (cbSize - (cbSize % BlockSize));
            }

            int bytesLeft = (cbSize % BlockSize);
            if (bytesLeft != 0)
            {
                Array.Copy(array, ((cbSize - bytesLeft) + ibStart), m_baPartialBlockBuffer, 0, bytesLeft);
                m_iPartialBlockFill = bytesLeft;
            }
        }

        protected override byte[] HashFinal()
        {
            return ProcessFinalBlock(m_baPartialBlockBuffer, 0, m_iPartialBlockFill);
        }


        public override void Initialize()
        {
            m_lTotalBytesProcessed = 0L;
            m_iPartialBlockFill = 0;

            if (m_baPartialBlockBuffer == null)
            {
                m_baPartialBlockBuffer = new byte[BlockSize];
            }
        }

        protected abstract void ProcessBlock(byte[] inputBuffer, int inputOffset, int inputLength);

        protected abstract byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);

        #endregion
    }

    [CLSCompliant(false)]
    public partial class TigerManaged : BlockHashAlgorithm
    {
        #region Constructor(s)

        public TigerManaged()
            : base(64, 192)
        {
            Initialize();
        }

        #endregion

        #region Fields

        private ulong[] accu;
        private ulong[] x;

        #endregion

        #region Properties
        // Put your public properties (keyword: PUBLIC)
        #endregion

        #region Methods

        public override void Initialize()
        {
            base.Initialize();

            accu = new ulong[] { 0x0123456789ABCDEFUL, 0xFEDCBA9876543210UL, 0xF096A5B4C3B2E187UL };
            if (x == null)
            {
                x = new ulong[8];
            }
            else
            {
                Array.Resize(ref x, 8);
            }

            Array.Clear(x, 0, 8);
        }

        private void KeySchedule(ref ulong x0, ref ulong x1, ref ulong x2, ref ulong x3, ref ulong x4, ref ulong x5, ref ulong x6, ref ulong x7)
        {
            x0 -= x7 ^ 0xA5A5A5A5A5A5A5A5UL;
            x1 ^= x0;
            x2 += x1;
            x3 -= x2 ^ ((~x1) << 19);
            x4 ^= x3;
            x5 += x4;
            x6 -= x5 ^ ((ulong)(~x4) >> 23);
            x7 ^= x6;
            x0 += x7;
            x1 -= x0 ^ ((~x7) << 19);
            x2 ^= x1;
            x3 += x2;
            x4 -= x3 ^ ((ulong)(~x2) >> 23);
            x5 ^= x4;
            x6 += x5;
            x7 -= x6 ^ 0x0123456789ABCDEFUL;
        }

        protected override void ProcessBlock(byte[] inputBuffer, int inputOffset, int inputLength)
        {
            ulong a = accu[0], b = accu[1], c = accu[2], x0, x1, x2, x3, x4, x5, x6, x7;

            int i, iSpaceNeeded = inputLength * 8;
            if (x.Length < iSpaceNeeded) Array.Resize(ref x, iSpaceNeeded);
            BitTools.TypeBlindCopy(inputBuffer, inputOffset, x, 0, inputLength * m_iInputBlockSize);

            for (i = -1; inputLength > 0; --inputLength, inputOffset += m_iInputBlockSize)
            {
                x0 = x[++i]; x1 = x[++i]; x2 = x[++i]; x3 = x[++i];
                x4 = x[++i]; x5 = x[++i]; x6 = x[++i]; x7 = x[++i];

                // rounds and schedule
                c ^= x0; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 5;
                a ^= x1; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 5;
                b ^= x2; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 5;
                c ^= x3; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 5;
                a ^= x4; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 5;
                b ^= x5; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 5;
                c ^= x6; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 5;
                a ^= x7; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 5;

                KeySchedule(ref x0, ref x1, ref x2, ref x3, ref x4, ref x5, ref x6, ref x7);

                b ^= x0; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 7;
                c ^= x1; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 7;
                a ^= x2; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 7;
                b ^= x3; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 7;
                c ^= x4; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 7;
                a ^= x5; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 7;
                b ^= x6; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 7;
                c ^= x7; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 7;

                KeySchedule(ref x0, ref x1, ref x2, ref x3, ref x4, ref x5, ref x6, ref x7);

                a ^= x0; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 9;
                b ^= x1; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 9;
                c ^= x2; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 9;
                a ^= x3; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 9;
                b ^= x4; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 9;
                c ^= x5; Round(ref a, ref b, (uint)(c >> 32), (uint)c); b *= 9;
                a ^= x6; Round(ref b, ref c, (uint)(a >> 32), (uint)a); c *= 9;
                b ^= x7; Round(ref c, ref a, (uint)(b >> 32), (uint)b); a *= 9;

                // feed forward
                a = accu[0] ^= a; b -= accu[1]; accu[1] = b; c = accu[2] += c;
            }
        }

        protected override byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
	        int paddingSize;

			// Figure out how much padding is needed between the last byte and the size.
			paddingSize = (int)(((ulong)inputCount + (ulong)m_lTotalBytesProcessed) % (ulong)BlockSize);
			paddingSize = (BlockSize - 8) - paddingSize;
			if(paddingSize < 1) { paddingSize += BlockSize; }

			// Create the final, padded block(s).
			if(inputOffset>0&&inputCount>0) Array.Copy(inputBuffer,inputOffset,inputBuffer,0,inputCount);
			inputOffset=0;

			Array.Clear(inputBuffer,inputCount,BlockSize-inputCount);
			inputBuffer[inputCount] = 0x01; //0x80;
			ulong msg_bit_length = ((ulong)m_lTotalBytesProcessed + (ulong)inputCount)<<3;

			if(inputCount+8 >= BlockSize)
			{
				if(inputBuffer.Length < 2*BlockSize) Array.Resize(ref inputBuffer,2*BlockSize);
				ProcessBlock(inputBuffer,inputOffset,1);
				inputOffset+=BlockSize; inputCount-=BlockSize;
			}

			for(inputCount=inputOffset+BlockSize-sizeof(ulong);msg_bit_length!=0;
					inputBuffer[inputCount]=(byte)msg_bit_length,msg_bit_length>>=8,++inputCount) ;
			ProcessBlock(inputBuffer,inputOffset,1);


			HashValue=new byte[HashSizeValue/8];
			BitTools.TypeBlindCopy(accu,0,HashValue,0,3);

            return HashValue;
		}

        private void Round(ref ulong x, ref ulong y, uint zh, uint zl)
        {
            x -= t1[(int)(byte)zl] ^ t2[(int)(byte)(zl >> 16)] ^ t3[(int)(byte)zh] ^ t4[(int)(byte)(zh >> 16)];
            y += t4[(int)(byte)(zl >> 8)] ^ t3[(int)(byte)(zl >> 24)] ^ t2[(int)(byte)(zh >> 8)] ^ t1[(int)(byte)(zh >> 24)];
        }

        #endregion
    }
}