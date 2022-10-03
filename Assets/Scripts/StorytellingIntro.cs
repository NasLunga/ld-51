using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Frame
{
    public Sprite background;
    public string charName;
    public string text;
    public Color charNameColor;

    public Frame(Sprite bg, string c, string t, Color col) {
        background = bg;
        charName = c;
        text = t;
        charNameColor = col;
    }
}

public class StorytellingIntro : MonoBehaviour
{
    public UnityEngine.UI.Image backgroundHolder;
    public TextMeshProUGUI charNameHolder;
    public TextMeshProUGUI textHolder;
    public List<Sprite> sprites;
    public List<Frame> frames;
    private int currentFrame = 0;
    private Coroutine drawTextTask;

    void Awake()
    {
        LoadFrames();
        DisplayNextFrame();
    }

    void LoadFrames()
    {
        frames = new List<Frame>();

        frames.Add(new Frame(
            sprites[0],
            "",
            "Finally found him...",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[1],
            "Knight",
            "Did you really think you could hide from me in this forest? Our kind knows this place like the backs of our hands.",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[1],
            "Criminal",
            "Please spare me! I didn't commit those crimes, the accusations are false! This is all a mistake!",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[1],
            "Knight",
            "Judgement of the royal court is absolute, there's no going against it. Justice always reaches you in the end",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[2],
            "",
            "",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[2],
            "Voice",
            "Noooooooo!",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[3],
            "Knight",
            "What's that?",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[4],
            "Witch",
            "How?... how dare you kill my precious boy? What made you think you are free to decide the fate of my son?",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[4],
            "Knight",
            "He was proven guilty by the royal court and sentenced to death. It was up to me to carry out the punishment.",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[4],
            "Witch",
            "Fool! I'll make you realize the mistake you've made.",
            new Color(1, 1, 1)
        ));

        frames.Add(new Frame(
            sprites[5],
            "",
            "",
            new Color(1, 1, 1)
        ));
    }

    void DisplayNextFrame() {
        Frame frame = frames[currentFrame];

        backgroundHolder.sprite = frame.background;
        charNameHolder.text = frame.charName;
        charNameHolder.color = frame.charNameColor;
        drawTextTask = StartCoroutine(DrawText(frame.text));

        currentFrame++;
    }

    IEnumerator DrawText(string text) {
        string displayedText = "";
        textHolder.text = displayedText;
        for (int i = 0; i < text.Length; i++) {
            displayedText += text[i];
            textHolder.text = displayedText;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (drawTextTask != null) {
                StopCoroutine(drawTextTask);
            }
            DisplayNextFrame();
        }
    }
}
