﻿using HttpMultipartParser;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Frends.Community.Multipart
{
    public static class MultipartTasks
    {
        /// <summary>
        /// A frends task form parsing multipart/form-data requests.
        /// Documentation: https://github.com/CommunityHiQ/Frends.Community.Multipart
        /// </summary>
        /// <param name="input">What to repeat.</param>
        /// <param name="cancellationToken"></param>
        ///  <returns>Object { List&lt;{ string Name, string Value }&gt; Parameters, List&lt;{ string Name, byte[] Contents }&gt; Files }</returns>
        public static async Task<Result> ParseMultipartRequest(Input input, CancellationToken cancellationToken)
        {
            var ret = new List<File>();
            var ret2 = new List<Parameter>();
            using (var stream = new MemoryStream(input.ByteArray))
            {
                var parser = await MultipartFormDataParser.ParseAsync(stream).ConfigureAwait(false);

                foreach (var file in parser.Files)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var tmp = new File {Name = file.FileName, Contents = ((MemoryStream) file.Data).ToArray()};
                    ret.Add(tmp);
                }

                foreach (var param in parser.Parameters)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var retParam = new Parameter
                    {
                        Name = param.Name,
                        Value = param.Data
                    };
                    ret2.Add(retParam);
                }
            }

            var output = new Result
            {
                Files = ret,
                Parameters = ret2
            };

            return output;
        }
    }
}