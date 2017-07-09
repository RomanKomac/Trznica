using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TagListViewAdapted : MonoBehaviour {

    [System.Serializable]
    public class Tag {
        public Color color;
        public string name;

        public Tag(Color color, string name) {
            this.color = color;
            this.name = name;
        }
    }

    public Color DefaultTagColor;

    public Tag[] tags;

    public Transform Content;
    public GameObject TagPrefab;

    private void Start() {
        foreach (Tag tag in tags) {
            GameObject _tag = Instantiate(TagPrefab) as GameObject;
            _tag.GetComponent<Image>().color = tag.color;
            _tag.transform.GetChild(0).GetComponent<Text>().text = tag.name;
            string name = tag.name.Equals("#all") ? "" : tag.name;
            _tag.GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<PodjetjeListViewAdapter>().FilterList(name));
            _tag.transform.SetParent(Content, false);
        }
    }

    public Color GetTagColor(string name) {
        Tag _tag = Array.Find(tags, tag => tag.name == name);
        return _tag != null ? _tag.color : DefaultTagColor;
    }
}
