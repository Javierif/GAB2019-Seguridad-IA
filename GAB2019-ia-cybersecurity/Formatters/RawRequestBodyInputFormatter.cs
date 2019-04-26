using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Formatters
{
    public class RawRequestBodyInputFormatter : InputFormatter
    {
        public override Boolean CanRead(InputFormatterContext context)
        {
            return true;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("face", out StringValues face);
            var request = context.HttpContext.Request;
            if (face == "true")
            {
                using (var reader = new StreamReader(request.Body))
                {
                    var content = await reader.ReadToEndAsync();
                    return await InputFormatterResult.SuccessAsync(content);
                }
            
            } else {
                MemoryStream streamReader;
                byte[] result;
                using (streamReader = new MemoryStream())
                {
                    request.Body.CopyTo(streamReader);
                    result = streamReader.ToArray();
                }

                return await InputFormatterResult.SuccessAsync(streamReader);

            }
        }
    }
}
