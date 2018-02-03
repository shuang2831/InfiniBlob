using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour {

    private Text[] textFields;
    private Image[] images;
    int score = 0;
    int health = 3;
    bool endGame;
    private Slider chargeSlider;                                 // Reference to the UI's health bar.
    // Use this for initialization
    void Start () {

        textFields = GetComponentsInChildren<Text>();

        getText("Score").text = "Score: " + score.ToString();
        getText("Health").text = health.ToString();
        getText("GameOver").text = "";
        getText("rText").text = "";
        //getText("cursor").text = ">";
    
        endGame = false;

        chargeSlider = GetComponentInChildren<Slider>();
        images = GetComponentsInChildren<Image>();

       
    }
	
	// Update is called once per frame
	void Update () {
        if (endGame)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("MiniGame");
            }
        }

    }

    private Text getText(string systemName)
    {
        foreach (Text tf in textFields)
        {
            if (tf.name == systemName)
            {
                return tf;
            }
        }
        return null;
    }

    private Image getImage(string systemName)
    {
        foreach (Image tf in images)
        {
            if (tf.name == systemName)
            {
                return tf;
            }
        }
        return null;
    }

    public void addScore(int amount)
    {
        score = score + amount;
        getText("Score").text = "Score: " + score.ToString();
    }
    public void setHealth(int currHealth)
    {
        health = currHealth;
        getText("Health").text = health.ToString();
        if (health <= 0)
        {
            getText("GameOver").text = "GAME OVER";
            getText("rText").text = "Press 'R' to Reset";
            endGame = true;
        }
    }
    public void setCharge(float amount)
    {
        chargeSlider.value = amount;
    }
    public void setCursor(Vector3 pos, float dir)
    {
        //getText("cursor").rectTransform.localEulerAngles = new Vector3(90f, 0f, 0);
        //getText("cursor").rectTransform.position = pos + new Vector3(75.0f, 1.0f, 1.0f);
        getImage("cursorImage").rectTransform.eulerAngles = new Vector3(0, 0, -dir);
        getImage("cursorImage").rectTransform.position = pos + new Vector3(0, 20.0f, 0.0f);
    }
    public float barVal()
    {
        Debug.Log(chargeSlider.value);
        Debug.Log(chargeSlider.maxValue);
        return (chargeSlider.value / chargeSlider.maxValue);
    }
}
