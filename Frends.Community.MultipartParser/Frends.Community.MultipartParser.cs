using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Microsoft.CSharp; // You can remove this if you don't need dynamic type in .NET Standard frends Tasks

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

        private static startpointCollection getStartpoints(byte[] ByteArray, byte[] boundaryArray, CancellationToken cancellationToken)
        {
            var ret = new startpointCollection();
            try
            {

                int ByteArrayLenght = ByteArray.Length;
                int hitcount = 0;
                var startpoints = new List<int>();

                var pointsAfterContentType = new List<int>();
                int linesAfterBoundary = 0;

                byte[] newLineArray = System.Text.Encoding.UTF8.GetBytes("\r\n");

                for (int i = 0; i < ByteArrayLenght; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (ByteArray[i] == boundaryArray[hitcount]) // if on boundary
                    {
                        hitcount++;
                    }
                    else // if not on boundary, start looking first byte of boundary
                    {
                        hitcount = 0;
                    }

                    if (hitcount == (boundaryArray.Length - 1)) // if on end of boundary add start point
                    {

                        startpoints.Add(i + 2); // newline or --
                        hitcount = 0;
                        linesAfterBoundary = 1;
                    }

                    if (i == ByteArrayLenght)
                    {
                        break;
                    } // Stop processing if end of array is reached. 

                    if ((linesAfterBoundary == 1 || linesAfterBoundary == 2 || linesAfterBoundary == 3) &&
                        ByteArray[i] == newLineArray[0] &&
                        ByteArray[i + 1] == newLineArray[1]) // look first lines after boundary 
                    {

                        if (linesAfterBoundary == 1 || linesAfterBoundary == 2)
                        {
                            linesAfterBoundary++;
                        }
                        else
                        {
                            pointsAfterContentType.Add(i + 2); // newline or --
                            linesAfterBoundary = 0;
                        }
                    }

                }



                ret.startpoints = startpoints;
                ret.pointsAfterContentType = pointsAfterContentType;

            }
            catch (Exception ex)
            {

                throw new Exception("Error in calculating startpoints", ex);

            }

            return ret;




        }
        /// <summary>
        /// This is task
        /// Documentation: https://github.com/CommunityHiQ/Frends.Community.MultipartParser
        /// </summary>
        /// <param name="input">What to repeat.</param>
        /// <param name="options">Define if repeated multiple times. </param>
        /// <param name="cancellationToken"></param>
        /// <returns>{string Replication} </returns>
        public static Result ParseMultipartBase64body(Parameters input, CancellationToken cancellationToken)
        {

            // If you develop this task, you might take look at https://github.com/Http-Multipart-Data-Parser/Http-Multipart-Data-Parser and take it to use.

            List<byte[]> ret = new List<byte[]>();
            List<string> ret2 = new List<string>();

            var BoundaryArray =
                System.Text.Encoding.UTF8.GetBytes("--" + input.boundaryFromHeader);

            var startAndCtpoints = getStartpoints(input.ByteArray, BoundaryArray, cancellationToken);


            var startpoints = startAndCtpoints.startpoints;
            var ctpoints = startAndCtpoints.pointsAfterContentType;


            try {
            for (int i = 0; i < 2; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int preLen = ctpoints[i] - startpoints[i] - 2;

                // Copy content
                var len = startpoints[i + 1] - startpoints[i] - BoundaryArray.Length - preLen - 6 - 1;
                var retdata = new byte[len];
                Array.Copy(input.ByteArray, startpoints[i] + preLen + 4, retdata, 0, len);

                ret.Add(retdata);

                // Copy header, e.g. Content-Type and Content-Disposition
                var retdataInformation = new byte[preLen]; // remove extra line brake between information and payload
                Array.Copy(input.ByteArray, startpoints[i] + 2, retdataInformation, 0, preLen);

                ret2.Add(System.Text.Encoding.UTF8.GetString(retdataInformation));
            }
            }
            catch (Exception ex)
            {

                throw new Exception("Error in forming result. startpoints: "+ String.Join(", ", startpoints) + " ctpoints: " + String.Join(", ", ctpoints),  ex);

            }


            var output = new Result
            {
                Contents = ret,
                ContentInformation = ret2
            };

            return output;
        }



    }
}