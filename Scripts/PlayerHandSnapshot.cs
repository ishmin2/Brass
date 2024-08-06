using System.Collections.Generic;
using Assets.Scripts.Client.BuildFeature;
using Assets.Scripts.Common.GameSaver.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerHandSnapshot : MonoBehaviour, ISnapshot
    {
        public List<DeckCardModel> Cards { get; }

        public PlayerHandSnapshot(List<DeckCardModel> cards)
        {
            Cards = new List<DeckCardModel>(cards);
        }
    }
}
