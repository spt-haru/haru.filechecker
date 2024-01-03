using System.Runtime.Serialization;

namespace Haru.FileChecker
{
    [DataContract]
    public struct ConsistencyMetadata
    {
        [DataMember]
        public string Path;

        [DataMember]
        public long Size;

        [DataMember(EmitDefaultValue = false)]
        public string Hash;

        [DataMember(EmitDefaultValue = false)]
        public int? Checksum;

        [DataMember(EmitDefaultValue = false)]
        public bool? IsCritical;

        public ConsistencyMetadata(string path, long size, string hash = null, int? checksum = null, bool? isCritical = null)
        {
            Path = path;
            Size = size;
            Hash = hash;
            Checksum = checksum;
            IsCritical = isCritical;
        }
    }
}