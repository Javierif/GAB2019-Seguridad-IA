using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Helpers
{
    public static class AudioHelper
{
    public static byte[] ReadFully(Stream input)
    {
            byte[] result;
            using (var streamReader = new MemoryStream())
            {
                input.CopyTo(streamReader);
                return result = streamReader.ToArray();
            }
                      
    }

}
}
