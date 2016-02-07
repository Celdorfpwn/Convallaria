using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace ThingSockets
{
    public static class Extensions
    {
        public static string GetMessage(this IInputStream stream)
        {
            try
            {
                var dataReader = new DataReader(stream);
                IAsyncOperation<uint> stringHeader = dataReader.LoadAsync(4);
                stringHeader.AsTask().Wait();
                var strLength = dataReader.ReadUInt32();
                IAsyncOperation<uint> taskLoad = dataReader.LoadAsync(strLength);
                taskLoad.AsTask().Wait();
                uint numStrBytes = taskLoad.GetResults();
                var message = dataReader.ReadString(numStrBytes);
                return message;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return String.Empty;
            }
            
        }

        public static bool WriteMessage(this IOutputStream stream, string message)
        {
            var writer = new DataWriter(stream);
            var len = writer.MeasureString(message);
            writer.WriteInt32((int)len);
            writer.WriteString(message);
            var written = true;
            try
            {
                written = writer.StoreAsync().AsTask().Wait(2000);
            }
            catch (AggregateException e)
            {
                Debug.WriteLine(e.ToString());
                written = false;
            }

            writer.DetachStream();
            return written;
        }

    }
}
