using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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
    private Toggle hoeToggle;
    private Toggle tntToggle;
    private Toggle mapToggle;

    private void Awake()
    {
        Instance = this;
        Transform iconHolder = transform.Find("BootmBar/IconHolder");
        Transform textHolder = transform.Find("BootmBar/TextHolder");
        levelText= transform.Find("BootmBar/LevelButton").GetComponent<Text>();
        armorIcon = iconHolder.Find("ArmorIcon").GetComponent<Image>();
        keyIcon = iconHolder.Find("KeyIcon").GetComponent<Image>();
        arrowBg = iconHolder.Find("ArrowBg").GetComponent<Image>();
        arrowIcon = iconHolder.Find("ArrowIcon").GetComponent<Image>();
        swordIcon = iconHolder.Find("SwordIcon").GetComponent<Image>();
        hoeIcon = iconHolder.Find("HoeIcon").GetComponent<Image>();
        hoeBag = iconHolder.Find("HoeIcon/BagIcon").GetComponent<Image>();
        tntIcon = iconHolder.Find("TNTIcon").GetComponent<Image>();
        tntBag = iconHolder.Find("TNTIcon/BagIcon").GetComponent<Image>();
        mapIcon = iconHolder.Find("MapIcon").GetComponent<Image>();
        mapBag = iconHolder.Find("MapIcon/BagIcon").GetComponent<Image>();
        grassIcon = iconHolder.Find("GrassIcon").GetComponent<Image>();
        hpText = textHolder.Find("HeartText").GetComponent<Text>(); 
        armorText = textHolder.Find("ArrmorText").GetComponent<Text>();
        keyText = textHolder.Find("KetText").GetComponent<Text>();
        weaponText = textHolder.Find("WeaponText").GetComponent<Text>();
        hoeText = textHolder.Find("HoeText").GetComponent<Text>();
        tntText = textHolder.Find("TNTText").GetComponent<Text>();
        mapText = textHolder.Find("Map").GetComponent<Text>();
        goldText = textHolder.Find("Gold").GetComponent<Text>();
    }

}
