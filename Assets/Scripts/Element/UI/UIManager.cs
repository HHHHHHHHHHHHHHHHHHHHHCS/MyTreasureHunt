using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private RectTransform rect;
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

    private void Awake()
    {
        Instance = this;
        rect = transform.Find("BottomBar").transform as RectTransform;
        Transform iconHolder = rect.Find("IconHolder");
        Transform textHolder = rect.Find("TextHolder");
        levelText = rect.Find("LevelButton").GetComponent<Text>();
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

        rect.Find("LevelButton").GetComponent<Button>().onClick.AddListener(OnLevelButtonClick);
        HoeToggle.onValueChanged.AddListener(OnHoeSelected);
        TntToggle.onValueChanged.AddListener(OnTNTSelected);
        MapToggle.onValueChanged.AddListener(OnMapSelected);
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
            rect.DOAnchorPosY(30f, 0.5f);
        }
        else
        {
            isHide = true;
            rect.DOAnchorPosY(-2.5f, 0.5f);
        }
    }

    public void OnUpdateUI()
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
}
