using System;
using System.Linq;
using Assets.Scripts.Client;
using Assets.Scripts.Common;
using Assets.Scripts.MainScene.Data;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Server
{
    public class ServerContainer
    {
        public Lazy<OuterMarketManager> OuterMarketManager { get; } = new Lazy<OuterMarketManager>(() => new OuterMarketManager());

        public Player ServerPlayer
        {
            get { return null; }
            // Object.FindObjectsOfType<Player>().Single(p => p.isServer && p.hasAuthority); }
        }

        private ServerContainer() { }

        private static ServerContainer _serverContainer;

        public static ServerContainer Instance()
        {
            if (_serverContainer == null)
            {
                _serverContainer = new ServerContainer();
            }

            return _serverContainer;
        }
    }
}
