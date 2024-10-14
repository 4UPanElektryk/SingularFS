namespace SingularFS
{
    internal interface IExportable
    {
        void Import(byte[] fileSystem);
        byte[] Export();
    }
}
