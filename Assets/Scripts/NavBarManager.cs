using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NavBarManager : MonoBehaviour {

    public GameObject compass;
    public GameObject NavBarDim; // [0, 128]
    private RectTransform navBar; // [-960, -150]
    public Shadow RazstavljalciShadow;
    public Shadow ZemljevidShadow;

    public GameObject ListView;
    public Image ContentImage;

    public GameObject target;

    public Transform TopBarHolder;
    public GameObject Search;

    private InputField searchField;

    public Sprite SearchIcon, TimesIcon;

    public GameObject TagSection;

    private bool isSearchShowing;

    private bool inTransition;

    private void Awake() {
        navBar = NavBarDim.transform.GetChild(0).GetComponent<RectTransform>();
        compass.SetActive(false);

        searchField = TopBarHolder.GetChild(1).GetComponent<InputField>();
    }

    public void OnMenuButtonClick() {
        if (inTransition) return;

        inTransition = true;
        NavBarDim.SetActive(true);
        NavBarDim.GetComponent<Image>().DOFade(0.5f, 1f).OnComplete(() => {
            inTransition = false;
        });
        navBar.DOLocalMoveX(-150, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void OnSearchClick() {
        if (!isSearchShowing) { 
            TopBarHolder.GetChild(0).gameObject.SetActive(false);
            TopBarHolder.GetChild(1).gameObject.SetActive(true);
            searchField.Select();
            Search.GetComponent<Image>().sprite = TimesIcon;
            isSearchShowing = true;
        } else {
            HideSearch();
        }
    }

    public void HideSearchButton() {
        Search.SetActive(false);
    }

    public void ShowSearchButton() {
        Search.SetActive(true);
    }

    public void HideTagSection() {
        TagSection.SetActive(false);
    }

    public void ShowTagSection() {
        TagSection.SetActive(true);
    }

    public void HideSearch() {
        TopBarHolder.GetChild(0).gameObject.SetActive(true);
        TopBarHolder.GetChild(1).gameObject.SetActive(false);
        Search.GetComponent<Image>().sprite = SearchIcon;
        isSearchShowing = false;
        searchField.text = "";
    }

    public void OnMenuHide() {
        if (inTransition) return;

        HideSearch();

        inTransition = true;
        navBar.DOLocalMoveX(-960, 0.5f).SetEase(Ease.InOutQuad);
        NavBarDim.GetComponent<Image>().DOFade(0f, 1f).OnComplete(() => {
            NavBarDim.SetActive(false);
            inTransition = false;
        });
    }

    public void OnRestavljalciButtonClick() {
        DOVirtual.Float(1, 0, 0.3f, (float value) => {
            Color color = ZemljevidShadow.effectColor;
            color.a = value;
            ZemljevidShadow.effectColor = color;
            color.a = 1 - value;
            RazstavljalciShadow.effectColor = color;
        });
        ContentImage.enabled = true;
        ListView.SetActive(true);

        FindObjectOfType<PodjetjeListViewAdapter>().HideCurrentPodjetje();
        FindObjectOfType<CameraMovement>()._isZemljevid = false;
        FindObjectOfType<CameraMovement>().target = target;

        compass.SetActive(false);
        ShowTagSection();
        ShowSearchButton();
    }

    public void OnZemljevidButtonClick() {
        DOVirtual.Float(1, 0, 0.3f, (float value) => {
            Color color = RazstavljalciShadow.effectColor;
            color.a = value;
            RazstavljalciShadow.effectColor = color;
            color.a = 1 - value;
            ZemljevidShadow.effectColor = color;
        });
        ListView.SetActive(false);
        ContentImage.enabled = false;

        HideSearch();
        HideSearchButton();

        FindObjectOfType<CameraMovement>()._isZemljevid = true;
        compass.SetActive(true);
        HideTagSection();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (NavBarDim.activeInHierarchy)
                OnMenuHide();
        }
    }
}
