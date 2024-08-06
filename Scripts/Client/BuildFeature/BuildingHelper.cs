using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Client.Commands.Models;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Client.BuildFeature
{
    public class BuildingHelper : MonoBehaviour
    {
        public static void HighlightGameObject(GameObject gameObject, bool active)
        {
            if (active)
            {
                Instantiate(Resources.Load("SelectionPrefab"), gameObject.transform);
            }
            else
            {
                var highlighter = gameObject.GetComponentsInChildren<Highlighter>().FirstOrDefault();
                if (highlighter != null)
                {
                    Destroy(highlighter.gameObject);
                }
                else
                {
                    Debug.Log($"Attempt to destroy Highlighter on {gameObject.name} that haven't it");
                }
            }
        }

        public static void HighlightGameObject(GameObject gameObject, bool active, string pathToResource)
        {
            if (active)
            {
                Instantiate(Resources.Load(pathToResource), gameObject.transform);
            }
            else
            {
                var highlighter = gameObject.GetComponentsInChildren<Transform>().Skip(1).First();
                Destroy(highlighter.gameObject);
            }
        }

        public static async Task AnimateBuildingSell(Building building, string resourcesPath)
        {
            await AnimateBuildingSell(building, resourcesPath, building.transform.position + new Vector3(0, 1));
        }

        public static async Task AnimateBuildingActivate(Building building)
        {
            var circle = 0;
            var angle = 0;
            while (circle != 5)
            {
                building.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
                if (angle % 360 == 0)
                {
                    circle++;
                }

                angle += 20;
                await Task.Delay(20);
            }
        }

        public static async Task ActivateBuilding(Building building)
        {
            var sr = building.GetComponent<SpriteRenderer>();
            // building.PlayerColor = building.PlayerColor == 0 ? Container.GetLocalPlayer().PlayerColor : building.PlayerColor;

            building.isActivated = true;
            Container.GetMusicManager().Play(SoundConstants.LevelUp);
            await AnimateBuildingActivate(building);
            var newSpriteName = sr.sprite.name.Replace("def", "act");
            var newSpite = Resources.Load<Sprite>($"AllSprites/{building.PlayerColor}/{newSpriteName}");
            sr.sprite = newSpite;
            Container.GetMusicManager().Play(SoundConstants.CashIncome);
            await AnimateBuildingSell(building, "Prefabs/Materials/Money");
            building.transform.rotation = new Quaternion(0, 0, 0, 0);

            Container.GetMoneyManager().ChangeIncome(building.IncomeValue);
            ClientEventAggregator.Publish(new ChangeMoneyRequestModel
            {
                // PlayerColor = Container.GetLocalPlayer().PlayerColor,
                Money = Container.Instance().GetPlayerPanel().LocalMoney,
                IncomeTrackPosition = Container.Instance().GetPlayerPanel().IncomeTrackPosition
            });

            building.SetIdleState();
        }

        private static async Task AnimateBuildingSell(Building building, string resourcesPath, Vector3 to)
        {
            var objPrefab = Resources.Load<GameObject>(resourcesPath);
            var objectInstance = Instantiate(objPrefab, building.gameObject.transform);

            var from = objectInstance.transform.position;

            while (to.y - from.y > 0.01f)
            {
                from = objectInstance.transform.position;
                objectInstance.transform.position = Vector3.MoveTowards(from, to, Time.deltaTime * 5);
                await Task.Delay(20);
            }

            Destroy(objectInstance);
        }
    }
}
