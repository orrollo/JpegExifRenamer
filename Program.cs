using System;
using System.Linq;
using System.IO;
using ExifLib;

namespace JpegExifRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Environment.CurrentDirectory;
            Console.WriteLine("scan <{0}> for jpeg images...", currentDirectory);
            var allFiles = Directory.GetFiles(currentDirectory);
            var jpegs = allFiles.Where(x =>
                {
                    var name = x.ToLower();
                    return name.EndsWith(".jpg") || name.EndsWith(".jpeg");
                }).ToArray();
            Console.WriteLine("found files: {0}", jpegs.Length);
            foreach (var jpeg in jpegs)
            {
                var fileName = Path.GetFileName(jpeg);
                try
                {
                    Console.WriteLine("processing {0}... ", fileName);
                    DateTime taken;
                    using (var rdr = new ExifReader(jpeg))
                    {
                        if (!rdr.GetTagValue<DateTime>(ExifTags.DateTimeDigitized,
                                                       out taken))
                        {
                            Console.WriteLine("unable to read picture date, skip.");
                            continue;
                        }
                    }
                    Console.WriteLine("picture taken: {0}", taken);
                    // split to directore structure
                    var path = currentDirectory;
                    var time = "";
                    for (int idx = 0; idx < 3; idx++)
                    {
                        var fmt = idx == 0 ? "{0:d4}" : "{0:d2}";
                        int datePart, timePart;
                        switch (idx)
                        {
                            case 0:
                                datePart = taken.Year;
                                timePart = taken.Hour;
                                break;
                            case 1:
                                datePart = taken.Month;
                                timePart = taken.Minute;
                                break;
                            default:
                                datePart = taken.Day;
                                timePart = taken.Second;
                                break;
                        }
                        path = Path.Combine(path, string.Format(fmt, datePart));
                        time = time + string.Format("{0:d2}", timePart);
                    }
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    // moving/renaming
                    var destFileName = Path.Combine(path, string.Format("{0}_{1}", time, fileName));
                    Console.WriteLine("moving to: {0}", destFileName);
                    File.Move(jpeg, destFileName);
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    if (ex.InnerException != null)
                        message += "; " + ex.InnerException.Message;
                    Console.WriteLine("error processing {0}: {1}", fileName, message);
                }
            }
            //Console.ReadLine();
        }
    }
}
