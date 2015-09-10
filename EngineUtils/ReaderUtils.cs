/*
 *****************************************************************
 *                     Street Engine Project                     *
 *                                                               *
 * Author: greatmaes (2015)                                      *
 * URL: https://github.com/greatmaes                             *
 *                                                               *
 * Notes:                                                        *
 * StreetEngine is a non-profit server side emulator for the ga- *
 * -me StreetGear. This is mostly a project to learn how to make *
 * your own emulator as I don't have the knowledge to finish th- *
 * -is project. Feel free to contribute.                         *
 *                                                               *
 * Credits:                                                      *
 * https://github.com/itsexe (help, original project)            *
 * http://www.elitepvpers.com/forum/members/4193997-k1ramox.html *
 *                                                               *
 ***************************************************************** 
*/
namespace StreetEngine.EngineUtils
{
    using System;
    using System.IO;
    using System.Text;

    public class ReaderUtils
    {
        private BinaryReader m_reader;

        public long BytesAvailable
        {
            get
            {
                return m_reader.BaseStream.Length - m_reader.BaseStream.Position;
            }
        }

        public long Position
        {
            get
            {
                return m_reader.BaseStream.Position;
            }
        }

        public Stream BaseStream
        {
            get
            {
                return m_reader.BaseStream;
            }
        }

        public ReaderUtils()
        {
            m_reader = new BinaryReader(new MemoryStream(), Encoding.UTF8);
        }

        public ReaderUtils(Stream stream)
        {
            m_reader = new BinaryReader(stream, Encoding.UTF8);
        }

        public ReaderUtils(byte[] tab)
        {
            m_reader = new BinaryReader(new MemoryStream(tab), Encoding.UTF8);
        }

        private byte[] ReadBigEndianBytes(int count)
        {
            var bytes = new byte[count];
            int i;
            for (i = count - 1; i >= 0; i--)
                bytes[i] = (byte)BaseStream.ReadByte();
            return bytes;
        }

        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadBigEndianBytes(2), 0);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBigEndianBytes(4), 0);
        }

        public Int64 ReadLong()
        {
            return BitConverter.ToInt64(ReadBigEndianBytes(8), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBigEndianBytes(4), 0);
        }

        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadBigEndianBytes(2), 0);
        }

        public UInt32 ReadUInt()
        {
            return BitConverter.ToUInt32(ReadBigEndianBytes(4), 0);
        }

        public UInt64 ReadULong()
        {
            return BitConverter.ToUInt64(ReadBigEndianBytes(8), 0);
        }

        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return m_reader.ReadSByte();
        }

        public byte[] ReadBytes(int n)
        {
            return m_reader.ReadBytes(n);
        }

        public ReaderUtils ReadBytesInNewBigEndianReader(int n)
        {
            return new ReaderUtils(m_reader.ReadBytes(n));
        }

        public Boolean ReadBoolean()
        {
            return m_reader.ReadByte() == 1;
        }

        public Char ReadChar()
        {
            return (char)ReadUShort();
        }

        public Double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBigEndianBytes(8), 0);
        }

        public Single ReadSingle()
        {
            return BitConverter.ToSingle(ReadBigEndianBytes(4), 0);
        }

        public string ReadUTF()
        {
            ushort length = ReadUShort();
            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public string ReadString()
        {
            ushort length = ReadByte();
            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);

        }

        public string ReadUTF7BitLength()
        {
            int length = ReadInt();
            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public string ReadUTFBytes(ushort len)
        {
            byte[] bytes = ReadBytes(len);
            return Encoding.UTF8.GetString(bytes);
        }

        public void SkipBytes(int n)
        {
            int i;
            for (i = 0; i < n; i++)
            {
                m_reader.ReadByte();
            }
        }

        public void Seek(int offset, SeekOrigin seekOrigin)
        {
            m_reader.BaseStream.Seek(offset, seekOrigin);
        }

        public void Add(byte[] data, int offset, int count)
        {
            long pos = m_reader.BaseStream.Position;
            m_reader.BaseStream.Position = m_reader.BaseStream.Length;
            m_reader.BaseStream.Write(data, offset, count);
            m_reader.BaseStream.Position = pos;
        }

        public void Dispose()
        {
            m_reader.Dispose();
            m_reader = null;
        }
    }
}
