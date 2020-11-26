#pragma warning disable 1591

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Community.Multipart
{
    /// <summary>
    /// Class for giving multipart message.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Http multipart message as byte array. 
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        [DefaultValue("System.Convert.FromBase64String(#trigger.data.httpContentBytesInBase64)")]
        public byte[] ByteArray { get; set; }
    }

    public class Parameter
    {
        /// <summary>
        /// Name of parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of parameter.
        /// </summary>
        public string Value { get; set; }
    }

    public class File
    {
        /// <summary>
        /// File name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File content.
        /// </summary>
        public byte[] Contents { get; set; }
    }

    public class Result
    {
        /// <summary>
        /// Files found from message.
        /// </summary>
        public List<File> Files;

        /// <summary>
        /// Parameters found from message.
        /// </summary>
        public List<Parameter> Parameters;
    }
}
