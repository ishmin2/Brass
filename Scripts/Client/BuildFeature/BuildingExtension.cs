using Assets.Scripts.Common;

namespace Assets.Scripts.Client.BuildFeature
{
    public static class BuildingExtension
    {
        public static City GetCity(this Building building)
        {
            return building.GetComponentInParent<City>();
        }
    }
}
