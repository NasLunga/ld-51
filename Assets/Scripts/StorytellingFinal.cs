using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class StorytellingFinal : MonoBehaviour
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
        Color knightColor = new Color(1, 1, 1);

        frames.Add(new Frame(
            sprites[0],
            "Knight",
            "The time has come. I'll finally find the cure for my curse.",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[0],
            "Knight",
            "Only a few more steps.",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[1],
            "Knight",
            "I've had to betray everyone I knew to get down here.",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[1],
            "Knight",
            "But I simply couldn't go on living like this.",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[2],
            "Knight",
            "Here it is! Now I just need to use it and the curse will be gone.",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[2],
            "Knight",
            "...?",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[2],
            "Knight",
            "It's already been used! This thing is useless!",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[2],
            "Knight",
            "That witch deceived me!",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[2],
            "",
            "",
            knightColor
        ));

        frames.Add(new Frame(
            sprites[2],
            "",
            "THE END",
            knightColor
        ));
    }

    void DisplayNextFrame() {
        if (currentFrame == frames.Count) {
            return;
        }

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
            if (currentFrame == frames.Count) {
                return;
            }
            if (drawTextTask != null) {
                StopCoroutine(drawTextTask);
            }
            DisplayNextFrame();
        }
    }
}
