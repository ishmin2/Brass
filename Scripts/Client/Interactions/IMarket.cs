namespace Assets.Scripts.Client.Interactions
{
    public interface IMarket
    {
        int CurrentCost { get; }

        int Buy();

        int Sell();
    }
}
