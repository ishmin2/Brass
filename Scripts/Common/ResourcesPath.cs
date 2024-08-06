namespace Assets.Scripts.Common
{
    public class ResourcesPath
    {
        public string Boat { get; }
        public string Railroad { get; }

        public string Avatar { get; }

        public static string PlayerInfoElement => "Prefabs/PageElements/PlayerInfoBar";

        private readonly PlayerColors _color;

        public ResourcesPath(PlayerColors color)
        {
            this._color = color;

            this.Boat = $"AllSprites/{this._color}/Boat";
            this.Railroad = $"AllSprites/{this._color}/Railroad";
            this.Avatar = $"AllSprites/{this._color}/PlayerA";
        }
    }
}
