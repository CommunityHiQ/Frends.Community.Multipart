#pragma warning disable 1591

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Community.MultipartParser
{
    /// <summary>
    /// Parameters class usually contains parameters that are required.
    /// </summary>
    public class Parameters
    {

        /// <summary>
        /// Something that will be repeated.
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        [DefaultValue("System.Convert.FromBase64String(#trigger.data.httpContentBytesInBase64)")]
        public byte[] ByteArray { get; set; }


        /// <summary>
        /// Something that will be repeated. #result.Headers["Content-Type"].Split(new string[] {"boundary="},StringSplitOptions.None)[1]
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        [DefaultValue("((string)#trigger.data.header[\"Content-Type\"]).Split(new string[] { \"boundary=\" }, StringSplitOptions.None)[1]")]
        public string boundaryFromHeader { get; set; }

    }



    public class Result
    {
        /// <summary>
        /// Contains the input repeated the specified number of times.
        /// </summary>
        public List<byte[]> Contents;

        /// <summary>
        /// Contains the input repeated the specified number of times.
        /// </summary>
        public List<string> ContentInformation;
    }
}
