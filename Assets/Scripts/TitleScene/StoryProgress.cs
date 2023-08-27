using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryProgress : MonoBehaviour
{
    [TextArea] public string[] story;

    public TextMeshProUGUI textObj;

    // Start is called before the first frame update
    void Start()
    {
        TypingManager.Instance.Typing(story, textObj);
    }

    // Update is called once per frame
    void Update()
    {
        if (TypingManager.isDialogEnd == true)
        {
            TitleSceneMover.StoryEnd = true;
        }
    }

}
