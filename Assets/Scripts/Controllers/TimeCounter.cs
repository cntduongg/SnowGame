using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    TextMeshProUGUI timeUI;

    float startTime;
    float elapsedTime;
    bool startCounter = false;

    int minutes;
    int seconds;

    void Start()
    {
        startCounter = false;
        timeUI = GetComponent<TextMeshProUGUI>();
    }

    public void StartTimeCounter()
    {
        startTime = Time.time;
        startCounter = true;
    }

    public void StopTimeCounter()
    {
        startCounter = false;
    }

    void Update()
    {
        if (startCounter)
        {
            elapsedTime = Time.time - startTime;
            minutes = (int)(elapsedTime / 60);
            seconds = (int)(elapsedTime % 60);
            timeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
