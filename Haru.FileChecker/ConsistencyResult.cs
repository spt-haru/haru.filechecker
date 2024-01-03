namespace Haru.FileChecker
{
    public enum ConsistencyResult
    {
        Success,
        FileNotFound,
        FileSizeMismatch,
        FileHashMismatch,
        FileChecksumMismatch
    }
}