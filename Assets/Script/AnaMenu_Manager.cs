﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Olcay;

public class AnaMenu_Manager : MonoBehaviour
{
    BellekYonetim _Bellekyonetim = new BellekYonetim();
    VeriYonetimi _VeriYonetim = new VeriYonetimi();
    ReklamYonetim _ReklamYonetim = new ReklamYonetim();
    public GameObject CikisPaneli;
  
    public AudioSource ButonSes;
    [Header("------ITEM VERİLERİ")]
    public List<ItemBilgileri> _Varsayilan_ItemBilgileri = new List<ItemBilgileri>();

    [Header("------DİL VERİLERİ")]   
    public List<DilVerileriAnaObje> _Varsayilan_DilVerileri = new List<DilVerileriAnaObje>();
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();    
    public Text[] TextObjeleri;

    [Header("------SAHNE YUKLEME OBJELERİ")]
    public GameObject YuklemeEkrani;
    public Slider YuklemeSlider;
    void Start()
    {
        _Bellekyonetim.KontrolEtveTanimla();
        _VeriYonetim.ilkKurulumDosyaOlusturma(_Varsayilan_ItemBilgileri,_Varsayilan_DilVerileri);
        ButonSes.volume = _Bellekyonetim.VeriOku_f("MenuFx");         

        _VeriYonetim.Dil_Load();
        _DilOkunanVeriler = _VeriYonetim.DilVerileriListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[0]);
        DilTercihiYonetimi();

    }    
    void DilTercihiYonetimi()
    {

        if (_Bellekyonetim.VeriOku_s("Dil") == "TR")
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_TR[i].Metin;
            }

        }
        else
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_EN[i].Metin;
            }
        }

    }
    public void SahneYukle(int Index)
    {
        ButonSes.Play();
        SceneManager.LoadScene(Index);

    }
    public void Oyna()
    {
        ButonSes.Play();      

        StartCoroutine(LoadAsync(_Bellekyonetim.VeriOku_i("SonLevel")));
    }
    IEnumerator LoadAsync(int SceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);

        YuklemeEkrani.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            YuklemeSlider.value = progress;
            yield return null;
        }

    }    
    public void CikisButonislem(string durum)
    {
        ButonSes.Play();
        if (durum == "Evet")
            Application.Quit();
        else if (durum == "cikis")
            CikisPaneli.SetActive(true);
        else
            CikisPaneli.SetActive(false);

    }
}
