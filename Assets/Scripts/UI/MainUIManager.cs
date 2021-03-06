﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum UIType
{
    Lv,
    Hp,
    Armor,
    Sword,
    Map,
    Arrow,
    Key,
    Tnt,
    Hoe,
    Grass,
    Gold
}

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager Instance { get; private set; }

    private RectTransform bottomPanel, passPanel, endPanel;
    private Image armorIcon;
    private Image keyIcon;
    private Image arrowBg;
    private Image arrowIcon;
    private Image swordIcon;
    private Image hoeIcon;
    private Image hoeBag;
    private Image tntIcon;
    private Image tntBag;
    private Image mapIcon;
    private Image mapBag;
    private Image grassIcon;
    private Text levelText;
    private Text hpText;
    private Text armorText;
    private Text keyText;
    private Text weaponText;
    private Text hoeText;
    private Text tntText;
    private Text mapText;
    private Text goldText;
    public Toggle HoeToggle { get; private set; }
    public Toggle TntToggle { get; private set; }
    public Toggle MapToggle { get; private set; }

    private bool isHide;

    public void OnInit()
    {
        Instance = this;
        bottomPanel = transform.Find("BottomPanel").transform as RectTransform;
        Transform iconHolder = bottomPanel.Find("IconHolder");
        Transform textHolder = bottomPanel.Find("TextHolder");
        levelText = bottomPanel.Find("LevelButton").GetComponent<Text>();
        armorIcon = iconHolder.Find("ArmorIcon").GetComponent<Image>();
        keyIcon = iconHolder.Find("KeyIcon").GetComponent<Image>();
        arrowBg = iconHolder.Find("ArrowBg").GetComponent<Image>();
        arrowIcon = iconHolder.Find("ArrowIcon").GetComponent<Image>();
        swordIcon = iconHolder.Find("SwordIcon").GetComponent<Image>();
        hoeIcon = iconHolder.Find("HoeIcon").GetComponent<Image>();
        hoeBag = hoeIcon.transform.Find("BagMask").GetComponent<Image>();
        tntIcon = iconHolder.Find("TNTIcon").GetComponent<Image>();
        tntBag = tntIcon.transform.Find("BagMask").GetComponent<Image>();
        mapIcon = iconHolder.Find("MapIcon").GetComponent<Image>();
        mapBag = mapIcon.transform.Find("BagMask").GetComponent<Image>();
        grassIcon = iconHolder.Find("GrassIcon").GetComponent<Image>();
        hpText = textHolder.Find("HeartText").GetComponent<Text>();
        armorText = textHolder.Find("ArmorText").GetComponent<Text>();
        keyText = textHolder.Find("KeyText").GetComponent<Text>();
        weaponText = textHolder.Find("WeaponText").GetComponent<Text>();
        hoeText = textHolder.Find("HoeText").GetComponent<Text>();
        tntText = textHolder.Find("TNTText").GetComponent<Text>();
        mapText = textHolder.Find("MapText").GetComponent<Text>();
        goldText = textHolder.Find("GoldText").GetComponent<Text>();
        HoeToggle = iconHolder.Find("HoeIcon").GetComponent<Toggle>();
        TntToggle = iconHolder.Find("TNTIcon").GetComponent<Toggle>();
        MapToggle = iconHolder.Find("MapIcon").GetComponent<Toggle>();

        passPanel = transform.Find("PassPanel").transform as RectTransform;
        var nextLevelButton = passPanel.Find("Bg/NextLevelButton").GetComponent<Button>();

        endPanel = transform.Find("EndPanel").transform as RectTransform;
        var exitButton = endPanel.Find("Bg/ExitButton").GetComponent<Button>();

        var audioMuteButton = bottomPanel.Find("AudioMuteButton").GetComponent<Button>();

        bottomPanel.Find("LevelButton").GetComponent<Button>().onClick.AddListener(OnLevelButtonClick);
        HoeToggle.onValueChanged.AddListener(OnHoeSelected);
        TntToggle.onValueChanged.AddListener(OnTNTSelected);
        MapToggle.onValueChanged.AddListener(OnMapSelected);
        nextLevelButton.onClick.AddListener(() => { AudioManager.Instance.PlayClip(AudioManager.Instance.button); MainGameManager.Instance.ChangeScene(SceneManager.GetActiveScene().name); });
        exitButton.onClick.AddListener(() => { AudioManager.Instance.PlayClip(AudioManager.Instance.button); MainGameManager.Instance.ChangeScene("menu"); });
        audioMuteButton.onClick.AddListener(() => { AudioManager.Instance.PlayClip(AudioManager.Instance.button); AudioManager.Instance.SwitchMuteState(); });
    }

    private void Start()
    {
        OnUpdateUI();
    }


    private void OnLevelButtonClick()
    {
        if (isHide)
        {
            isHide = false;
            bottomPanel.DOAnchorPosY(30f, 0.5f);
        }
        else
        {
            isHide = true;
            bottomPanel.DOAnchorPosY(-2.5f, 0.5f);
        }
    }

    public void OnUpdateUI(params UIType[] uiType)
    {
        var manager = MainGameManager.Instance;
        levelText.text = "Level:" + manager.Lv;
        hpText.text = manager.Hp.ToString();

        armorIcon.gameObject.SetActive(manager.Armor != 0);
        armorText.gameObject.SetActive(manager.Armor != 0);
        armorText.text = manager.Armor.ToString();

        keyIcon.gameObject.SetActive(manager.Key != 0);
        keyText.gameObject.SetActive(manager.Key != 0);
        keyText.text = manager.Key.ToString();

        hoeIcon.gameObject.SetActive(manager.Hoe != 0);
        hoeText.gameObject.SetActive(manager.Hoe != 0);
        hoeText.text = manager.Hoe.ToString();

        tntIcon.gameObject.SetActive(manager.Tnt != 0);
        tntText.gameObject.SetActive(manager.Tnt != 0);
        tntText.text = manager.Tnt.ToString();

        mapIcon.gameObject.SetActive(manager.Map != 0);
        mapText.gameObject.SetActive(manager.Map != 0);
        mapText.text = manager.Map.ToString();

        grassIcon.gameObject.SetActive(manager.IsGrass);
        goldText.text = manager.Gold.ToString();

        switch (manager.WeaponType)
        {
            case WeaponType.None:
                arrowBg.gameObject.SetActive(true);
                arrowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(false);
                weaponText.gameObject.SetActive(false);
                break;
            case WeaponType.Arrow:
                arrowBg.gameObject.SetActive(false);
                arrowIcon.gameObject.SetActive(true);
                swordIcon.gameObject.SetActive(false);
                weaponText.gameObject.SetActive(true);
                weaponText.text = manager.Arrow.ToString();
                break;
            case WeaponType.Sword:
                arrowBg.gameObject.SetActive(false);
                arrowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(true);
                weaponText.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        foreach (var item in uiType)
        {
            RectTransform rt = null;
            switch (item)
            {
                case UIType.Lv:
                    rt = levelText.transform as RectTransform;
                    break;
                case UIType.Hp:
                    rt = hpText.transform as RectTransform;
                    break;
                case UIType.Armor:
                    rt = armorText.transform as RectTransform;
                    break;
                case UIType.Sword:
                    rt = swordIcon.transform as RectTransform;
                    break;
                case UIType.Map:
                    rt = mapIcon.transform as RectTransform;
                    break;
                case UIType.Arrow:
                    rt = arrowIcon.transform as RectTransform;
                    break;
                case UIType.Key:
                    rt = keyIcon.transform as RectTransform;
                    break;
                case UIType.Tnt:
                    rt = tntIcon.transform as RectTransform;
                    break;
                case UIType.Hoe:
                    rt = hoeIcon.transform as RectTransform;
                    break;
                case UIType.Grass:
                    rt = grassIcon.transform as RectTransform;
                    break;
                case UIType.Gold:
                    rt = goldText.transform as RectTransform;
                    break;
                default:
                    break;
            }
            rt.DOShakeScale(0.5f)
                .OnComplete(() => { rt.localScale = Vector3.one; });
        }
    }

    private void OnHoeSelected(bool isOn)
    {
        MainGameManager.Instance.HoeSelect.SetActive(isOn);
    }


    private void OnTNTSelected(bool isOn)
    {
        MainGameManager.Instance.TNTSelect.SetActive(isOn);
    }


    private void OnMapSelected(bool isOn)
    {
        MainGameManager.Instance.MapSelect.SetActive(isOn);
    }

    public void ShowPassPanel()
    {
        passPanel.gameObject.SetActive(true);
    }

    public void ShowEndPanel()
    {
        endPanel.gameObject.SetActive(true);
    }

    public void DoDestory()
    {
        Instance = null;
    }
}
