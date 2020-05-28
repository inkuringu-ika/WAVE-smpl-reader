using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpl_reader
{
    class Program
    {
        private static BinaryReader wavfileread;

        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Error: File not set.");
                return;
            }
            var filename = @args[0];
            try
            {
                wavfileread = new BinaryReader(new FileStream(filename, FileMode.Open));
            }
            catch
            {
                Console.WriteLine("Error: Failed to read the file.");
                return;
            }
            if (Encoding.GetEncoding(20127).GetString(wavfileread.ReadBytes(4)) == "RIFF")
            {
                //BitConverter.ToInt32(wavfileread.ReadBytes(4), 0).ToString()
                wavfileread.BaseStream.Seek(4, SeekOrigin.Current);
                if (Encoding.GetEncoding(20127).GetString(wavfileread.ReadBytes(4)) == "WAVE")
                {
                    while (wavfileread.BaseStream.Position != wavfileread.BaseStream.Length)
                    {
                        var chunkid = Encoding.GetEncoding(20127).GetString(wavfileread.ReadBytes(4));
                        if (chunkid == "fmt ")
                        {
                            var fmt_bytes = BitConverter.ToInt32(wavfileread.ReadBytes(4), 0);
                            wavfileread.BaseStream.Seek(2, SeekOrigin.Current);
                            var channel_count = BitConverter.ToInt16(wavfileread.ReadBytes(2), 0);
                            Console.WriteLine("channel_count: " + channel_count);
                            var sample_rate = BitConverter.ToInt32(wavfileread.ReadBytes(4), 0);
                            Console.WriteLine("sample_rate: " + sample_rate);
                            wavfileread.BaseStream.Seek(6, SeekOrigin.Current);
                            var bit = BitConverter.ToInt16(wavfileread.ReadBytes(2), 0);
                            Console.WriteLine("bit: " + bit);
                            wavfileread.BaseStream.Seek(fmt_bytes - 16, SeekOrigin.Current);
                        }
                        else if (chunkid == "smpl")
                        {
                            var smpl_bytes = BitConverter.ToInt32(wavfileread.ReadBytes(4), 0);
                            wavfileread.BaseStream.Seek(44, SeekOrigin.Current);
                            var loopstart = BitConverter.ToInt32(wavfileread.ReadBytes(4), 0);
                            Console.WriteLine("loopstart: " + loopstart);
                            var loopend = BitConverter.ToInt32(wavfileread.ReadBytes(4), 0);
                            Console.WriteLine("loopend: " + loopend);
                            wavfileread.BaseStream.Seek(4, SeekOrigin.Current);
                            var loopcount = BitConverter.ToInt32(wavfileread.ReadBytes(4), 0);
                            Console.WriteLine("loopcount: " + loopcount);
                        }
                        else
                        {
                            wavfileread.BaseStream.Seek(BitConverter.ToInt32(wavfileread.ReadBytes(4), 0), SeekOrigin.Current);
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Error: Invalid file.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid file.");
                return;
            }
        }
    }
}
