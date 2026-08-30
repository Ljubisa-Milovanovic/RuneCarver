using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    public class ButtonProps : MonoBehaviour
    {
        public GameData gameData;

        private enum ButtonAction
        {
            LoadScene,
            Exit,
            None
        };

        [SerializeField] private ButtonAction action;
        [SerializeField] private string sceneName;

        private Color _baseColor;
        private Image _image;
        private readonly Color _highlight = new(0.8549019607843137f, 0.8588235294117647f, 0.8666666666666667f, 0.5f);

        private void Start()
        {
            _image = transform.GetComponent<Image>();
            _baseColor = _image.color;
            Debug.Log(_highlight);
        }

        private void OnMouseEnter()
        {
            _image.color = _highlight;
        }

        private void OnMouseExit()
        {
            _image.color = _baseColor;
        }

        private void OnMouseDown()
        {
            HandleClick();
        }

        private void HandleClick()
        {
            switch (action)
            {
                case ButtonAction.LoadScene:
                    if (!string.IsNullOrEmpty(sceneName))
                    {
                        gameData.lastLoadedScene = SceneManager.GetActiveScene().name;
                        SceneManager.LoadScene(sceneName);
                    }
                    else
                        Debug.LogWarning($"{gameObject.name}: scene isn't set");
                    break;

                case ButtonAction.Exit:
                    PlayerPrefs.SetInt("currentHP", gameData.currentHP);
                    PlayerPrefs.SetString("currentStage", gameData.currentStage);
                    PlayerPrefs.SetInt("currentMana", gameData.currentMana);
                    Application.Quit();
                    Debug.Log("Quit");
                    break;

                case ButtonAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}