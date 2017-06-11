using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : MonoBehaviour {

    public GameObject Dimmer;

    private Text title;
    private Text message;

	void Awake () {
        title = Dimmer.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        message = Dimmer.transform.GetChild(0).GetChild(2).GetComponent<Text>();
    }

    public void OTrznici() {
        title.text = "O tržnici";
        message.text = "20. let sejmov. Parkiranje je brezplačno in organizirano. Vstop je prost.";
        ShowAlertDialog();
    }

    public void Organizator() {
        title.text = "Organizator";
        message.text = "Konjeniški klub Komenda\nGlavarjeva cesta 98, 1218 Komenda";
        ShowAlertDialog();
    }

    public void OAplikaciji() {
        title.text = "O aplikaciji";
        message.text = "Aplikacija narejena v sklopu predmeta Osnovne oblikovanja.\n\nRoman Komac, Žan Ožbot";
        ShowAlertDialog();
    }

    public void ShowAlertDialog() {
        Dimmer.SetActive(true);
    }

    public void HideAlertDialog() {
        Dimmer.SetActive(false);
    }

}
