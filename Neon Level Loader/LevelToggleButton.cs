using UnityEngine;

namespace RMMBY.NeonLevelLoader
{
    public class LevelToggleButton : MonoBehaviour
    {
        private MenuHandler sys;

        private bool isHighlighted;
        private GameObject highlight;
        private GameObject unHighlight;

        private void Start()
        {
            sys = GameObject.FindObjectOfType<MenuHandler>();
            highlight = transform.Find("Highlight").gameObject;
            if (gameObject.name.StartsWith("Button"))
                unHighlight = transform.Find("UnHighlight").gameObject;
            else if (gameObject.name.StartsWith("Option"))
                unHighlight = transform.Find("ChoiceBack").gameObject;
        }

        private void Update() // uhhh wish i could scrap this xD
        {
            if (!isHighlighted && sys.selectedObject == gameObject)
            {
                isHighlighted = true;
                highlight.SetActive(true);
                unHighlight.SetActive(false);
                return;
            }
            if (isHighlighted && sys.selectedObject != gameObject)
            {
                highlight.SetActive(false);
                isHighlighted = false;
                unHighlight.SetActive(true);
            }
        }
    }
}
