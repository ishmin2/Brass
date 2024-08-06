namespace Assets.Scripts.Client.BuildFeature
{
    public interface ISnapshotable
    {
        ISnapshot Save();

        void Restore(ISnapshot snapshot);
    }
}
