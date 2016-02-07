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
        public static string GetMessage(this DataReader dataReader)
        {
            IAsyncOperation<uint> stringHeader = dataReader.LoadAsync(4);
            stringHeader.AsTask().Wait();
            var strLength = dataReader.ReadUInt32();
            IAsyncOperation<uint> taskLoad = dataReader.LoadAsync(strLength);
            taskLoad.AsTask().Wait(2000);
            uint numStrBytes = taskLoad.GetResults();
            var message = dataReader.ReadString(numStrBytes);
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            return message;
        }

        public static bool WriteMessage(this DataWriter writer, string message)
        {
            var len = writer.MeasureString(message);
            writer.WriteInt32((int)len);
            writer.WriteString(message);
            var written = writer.StoreAsync().AsTask().Wait(2000);
            return written;
        }

    }
}
