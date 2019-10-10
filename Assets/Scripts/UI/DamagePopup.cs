using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public Transform damagePopupPrefab;
    private TextMeshPro tm;
    private float fadeTimer;
    private Color textColor;

    public static DamagePopup Create(Vector3 position, int amount, bool crit)
    {
		Debug.Log("executed DamagePopup.Create()"); //V
		
		Transform popupTransform = Instantiate(damagePopupPrefab, position); //V
        DamagePopup damagePopup = popupTransform.GetComponent<DamagePopup>(); //V
		
        // Transform popupTransform = Instantiate(damagePopupPrefab, new Vector3(0,0,0), Quaternion.identity);
        // DamagePopup damagePopup = popupTransform.GetComponent<DamagePopup>();

        // return damagePopup;
		
        return null;
    }

    private void Awake()
    {
        tm = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int amount, bool crit)
    {
        tm.SetText(amount.ToString());

        if (!crit)
        {
            tm.fontSize = 36;
            textColor = Color.red;
        }
        else
        {
            tm.fontSize = 42;
            textColor = Color.yellow;
        }
        tm.color = textColor;
        fadeTimer -= Time.deltaTime;

    }


    private void Update()
    {
        float moveSpeed = 20f;
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        fadeTimer -= Time.deltaTime;
        if(fadeTimer < 0)
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            tm.color = textColor;

            if(textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
