using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PodjetjeListViewAdapter : MonoBehaviour {

    public RectTransform Content;
    public GameObject PodjetjePrefab;

    public List<Podjetje> podjetja;

    [System.Serializable]
    public class Podjetje {
        public Sprite logo;
        //[HideInInspector]
        //public int id;
        public string ime;
        public string tag1;
        public string tag2;

        public Podjetje(Sprite logo, string ime, string tag1, string tag2) {
            this.logo = logo;
            this.ime = ime;
            this.tag1 = tag1;
            this.tag2 = tag2;
        }

        //public static int count = 0;

        //public int _id { get { count++; return count; } set { count = value; } }
    }

	void Start () {
        foreach (Podjetje podjetje in podjetja) {
            GameObject novoPodjetje = Instantiate(PodjetjePrefab) as GameObject;

            novoPodjetje.GetComponent<Button>().onClick.AddListener(() => OnPodjetjeClick(podjetje.ime));

            novoPodjetje.transform.GetChild(0).GetComponent<Image>().sprite = podjetje.logo;
            novoPodjetje.transform.GetChild(1).GetComponent<Text>().text = podjetje.ime;
            novoPodjetje.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = podjetje.tag1;
            novoPodjetje.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = podjetje.tag2;

            novoPodjetje.transform.SetParent(Content.transform, false);
        }        
    }

    public void OnPodjetjeClick(string ime) {
        print(ime);
        FindObjectOfType<NavBarManager>().OnZemljevidButtonClick();
        //FindObjectOfType<CameraMovement>().SetTarget(GameObject.Find(ime));
    }
}
