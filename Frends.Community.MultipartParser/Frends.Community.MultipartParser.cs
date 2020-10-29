using HttpMultipartParser;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Frends.Community.MultipartParser
{
    internal class startpointCollection
    {
        /// <summary>
        /// Contains the input repeated the specified number of times.
        /// </summary>
        internal List<int> startpoints;
        internal List<int> pointsAfterContentType;
    }

    public static class MultipartTasks
    {
        /// <summary>
        /// This is task
        /// Documentation: https://github.com/CommunityHiQ/Frends.Community.MultipartParser
        /// </summary>
        /// <param name="input">What to repeat.</param>
        /// <param name="options">Define if repeated multiple times. </param>
        /// <param name="cancellationToken"></param>
        /// <returns>{string Replication} </returns>
        public static async Task<Result> ParseMultipartBase64body(Input input, CancellationToken cancellationToken)
        {
            var ret = new List<File>();
            var ret2 = new List<Parameter>();

            MemoryStream stream = new MemoryStream(input.ByteArray);

            var parser = await MultipartFormDataParser.ParseAsync(stream).ConfigureAwait(false);

            foreach (var file in parser.Files)
            {
                var tmp = new File { Name = file.FileName, Contents = ((MemoryStream)file.Data).ToArray() };
                ret.Add(tmp);
            }

            foreach (var param in parser.Parameters)
            {
                var retParam = new Parameter
                {
                    Name = param.Name,
                    Value = param.Data
                };
                ret2.Add(retParam);
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