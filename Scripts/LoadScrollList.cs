using System.IO;
using Assets.Scripts.Common.GameSaver;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;

namespace Assets.Scripts
{
    public class LoadScrollList : MonoBehaviour
    {
        public GameObject Container;

        void Awake()
        {
            var button = Resources.Load<GameObject>("LoadButton");
            var saveFiles = Directory.GetFiles(Application.persistentDataPath, "*.sav");

            foreach (var save in saveFiles)
            {
                var instance = Instantiate(button, this.Container.transform);
                instance.GetComponentInChildren<Text>().text = Path.GetFileName(save).Replace(".sav", string.Empty);
                instance.GetComponent<Button>().onClick.AddListener(() => new LoadManager().Load(File.ReadAllText(save)));
            }
        }
    }
}
