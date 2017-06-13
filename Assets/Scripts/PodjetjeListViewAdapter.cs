using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PodjetjeListViewAdapter : MonoBehaviour {

    public RectTransform Content;
    public GameObject PodjetjePrefab;

    public List<Podjetje> podjetja;

    private Podjetje[] arrayPodjetja;

    [System.Serializable]
    public class Podjetje {
        public Sprite logo;
        //[HideInInspector]
        public int id;
        public GameObject target;
        public string ime;
        public string tag1;
        public string tag2;

        public Podjetje(Sprite logo, int id, GameObject target, string ime, string tag1, string tag2) {
            this.logo = logo;
            this.id = id;
            this.target = target;
            this.ime = ime;
            this.tag1 = tag1;
            this.tag2 = tag2;
        }

        //public static int count = 0;

        //public int _id { get { count++; return count; } set { count = value; } }
    }

    private GameObject currentPodjetjeShowing;

	void Start () {
        arrayPodjetja = podjetja.ToArray();

        foreach (Podjetje podjetje in podjetja) {
            GameObject novoPodjetje = Instantiate(PodjetjePrefab) as GameObject;

            novoPodjetje.GetComponent<Button>().onClick.AddListener(() => OnPodjetjeClick(podjetje.id));

            podjetje.target.transform.parent.gameObject.SetActive(false);

            novoPodjetje.transform.GetChild(0).GetComponent<Image>().sprite = podjetje.logo;
            novoPodjetje.transform.GetChild(1).GetComponent<Text>().text = podjetje.ime;
            novoPodjetje.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = podjetje.tag1;
            novoPodjetje.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = podjetje.tag2;

            novoPodjetje.transform.SetParent(Content.transform, false);
        }        
    }

    public void HideCurrentPodjetje() {
        if (currentPodjetjeShowing != null) {
            currentPodjetjeShowing.SetActive(false);
            currentPodjetjeShowing = null;
        }
    }

    public void OnPodjetjeClick(int id) {
        currentPodjetjeShowing = arrayPodjetja[id].target.transform.parent.gameObject;

        FindObjectOfType<NavBarManager>().OnZemljevidButtonClick();
        currentPodjetjeShowing.SetActive(true);
        FindObjectOfType<CameraMovement>().SetTarget(arrayPodjetja[id].target);
    }
}
