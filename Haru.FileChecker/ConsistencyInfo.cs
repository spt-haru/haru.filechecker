using System.Runtime.Serialization;
using Haru.Shared;

namespace Haru.FileChecker
{
    [DataContract]
    public struct ConsistencyInfo
    {
        [DataMember]
        public string Version;

        [DataMember]
        public ConsistencyMetadata[] Entries;

        public ConsistencyInfo(ConsistencyMetadata[] entries)
        {
            Version = SharedConsts.GameVersion;
            Entries = entries;
        }
    }
}