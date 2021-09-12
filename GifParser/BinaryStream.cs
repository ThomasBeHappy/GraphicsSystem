using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GifParser
{
    /// <summary>
    /// This class allows reading from a byte array single bits
    /// </summary>
    public class BinaryStream : Stream
    {
        private byte[] data;
        private long bytePos;
        private byte bitPos;

        public BinaryStream(byte[] Data)
        {
            data = Data;
            bytePos = 0;
            bitPos = 0;
        }

        public override bool CanRead => data.Length != bytePos;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        /// <summary>
        /// Length in bits
        /// </summary>
        public override long Length => data.Length * 8;

        public override long Position
        {
            get => bytePos * 8 + bitPos;
            set
            {
                bitPos = (byte)(value % 8);
                bytePos = value / 8;
            }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Will read count number of bits. The first byte will be filled from LSB first. So the first byte migth not be fully filled but all
        /// later ones will be
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count">Number of bits to read</param>
        /// <returns>Number of bytes read rounded up</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
            {
                return 0;
            }
            var bytesToRead = (int)Math.Min(buffer.Length - offset, data.Length - bytePos);
            var bitsToRead = Math.Min(Math.Min(bytesToRead * 8, Length - bytePos * 8 - bitPos), count);
            buffer[offset] = 0;
            for (int i = 0; i < bitsToRead % 8; i++)
            {
                buffer[offset] <<= 1;
                buffer[offset] |= ReadBit();
            }
            if (bitsToRead % 8 != 0)
            {
                offset++;
            }
            for (int i = 0; i < bitsToRead / 8; i++)
            {
                buffer[offset++] = ReadByte();
            }


            return bytesToRead;
        }

        /// <summary>
        /// Read one byte from the stream
        /// </summary>
        /// <returns></returns>
        private byte ReadByte()
        {
            if (bitPos == 0)
            {
                return data[bytePos++];
            }
            else
            {
                byte val = 0;
                for (int i = 0; i < 8; i++)
                {
                    val <<= 1;
                    val |= ReadBit();
                }
                return val;
            }
        }

        /// <summary>
        /// Read one bit from the stream
        /// </summary>
        /// <returns></returns>
        private byte ReadBit()
        {
            if (!CanRead)
            {
                throw new IOException();
            }
            var readByte = data[bytePos];
            byte bit = (byte)((readByte & (1 << (7 - bitPos))) >> (7 - bitPos));
            bitPos++;
            if (bitPos == 8)
            {
                bitPos = 0;
                bytePos++;
            }
            return bit;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    bytePos = offset / 8;
                    bitPos = (byte)(offset % 8);
                    break;
                case SeekOrigin.Current:
                    bytePos += offset / 8;
                    bitPos += (byte)(offset % 8);
                    break;
                case SeekOrigin.End:
                    bytePos = Length - offset / 8;
                    bitPos = (byte)(Length - offset % 8);
                    break;
                default:
                    break;
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
