using UnityEngine;

public class BlinkToggle : MonoBehaviour
{
    public float speed = 0.1f;
    float Delay;
    
    Color colorBegin;
    Color colorEnd;

    void Start()
    {
        colorBegin = GetComponent<Renderer>().material.color;
        colorEnd = new Color(colorBegin.r, colorBegin.g, colorBegin.b, 0f);
    }

    void Update()
    {
        Delay += Time.deltaTime;
        GetComponent<Renderer>().material.color = Color.Lerp(colorBegin, colorEnd, Delay * speed);

        if (System.Math.Abs(transform.GetComponent<Renderer>().material.color.a-colorBegin.a) < Mathf.Epsilon ||
            System.Math.Abs(transform.GetComponent<Renderer>().material.color.a-colorEnd.a) < Mathf.Epsilon)
        {
            Color temp = colorBegin;
            colorBegin = colorEnd;
            colorEnd = temp;
            Delay = 1f;
        }
    }
}