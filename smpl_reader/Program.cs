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
        static void Main(string[] args)
        {
            var filename = @args[0];
            //var filename = @"F:\任天堂ツール、音楽再生、変換\再生\.wav";
            var wavfileread = new BinaryReader(new FileStream(filename, FileMode.Open));
            if (Encoding.GetEncoding(20127).GetString(wavfileread.ReadBytes(4)) == "RIFF")
            {
                //BitConverter.ToInt32(wavfileread.ReadBytes(4), 0).ToString()
                wavfileread.BaseStream.Seek(4, SeekOrigin.Current);
                if (Encoding.GetEncoding(20127).GetString(wavfileread.ReadBytes(4)) == "WAVE")
                {
                    //Console.WriteLine("reading...");
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
                            wavfileread.BaseStream.Seek(fmt_bytes - 8, SeekOrigin.Current);
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
            }
        }
    }
}
