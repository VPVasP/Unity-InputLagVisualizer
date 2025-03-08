using UnityEngine;
using TMPro;
using System.Collections;

public class InputVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputLagText; //ui to detect input lag
    private Coroutine resetCoroutine; //coroutine that resets the text after a delay
    private readonly string defaultText = "Press any key to measure input lag"; //default display text

    void Start()
    {
        //set initial text on UI
        inputLagText.text = defaultText;
    }

    void Update()
    {
        //check if any key is pressed on the keyboard
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            KeyCode key = GetPressedKey();
            if (key != KeyCode.None)
            {
                MeasureInputLag(key);
            }
        }
    }

    //detects which key was pressed
    private KeyCode GetPressedKey()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
                return key; //returns the pressed key
        }
        return KeyCode.None; //returns none key if no key is pressed
    }

    private void MeasureInputLag(KeyCode key)
    {
        float inputTime = Time.realtimeSinceStartup; //recording the time when key is pressed
        StartCoroutine(DisplayInputLag(key, inputTime)); //starting our coroutine to display the key and the input time
    }

    private IEnumerator DisplayInputLag(KeyCode key, float inputTime)
    {
        yield return null; //Waiting for frame update

        float latency = (Time.realtimeSinceStartup - inputTime) * 1000f;//calcuulating the latency
        string message = $"Input lag for {key}: {latency:F2} ms"; //the message of input lag and latency

        inputLagText.text = message; //we set our UI text to the message of input lag and latency
        Debug.Log(message);

        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        resetCoroutine = StartCoroutine(ResetTextAfterDelay());
    }

    private IEnumerator ResetTextAfterDelay()
    {
        yield return new WaitForSeconds(2f); //wait for 2 seconds to update the UI
        inputLagText.text = defaultText;
    }
}