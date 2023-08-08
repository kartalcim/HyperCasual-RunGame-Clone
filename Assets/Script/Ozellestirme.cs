﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Olcay;
using TMPro;
using UnityEngine.SceneManagement;

public class Ozellestirme : MonoBehaviour
{
    public Text PuanText;
    
    public GameObject[] islemPanelleri;
    public GameObject islemCanvasi;
    public GameObject[] GenelPaneller;
    public Button[] islemButonlari;    
    int AktifislemPaneliIndex;
    [Header("-------SAPKALAR")]
    public GameObject[] Sapkalar;
    public Button[] SapkaButonlari;
    public Text SapkaText;
    [Header("-------SOPALAR")]
    public GameObject[] Sopalar;
    public Button[] SopaButonlari;
    public Text SopaText;
    [Header("-------MATERİAL")]
    public Material[] Materyaller;
    public Material VarsayilanTema;
    public Button[] MateryalButonlari;
    public Text MaterialText;
    public SkinnedMeshRenderer _Renderer;

    int SapkaIndex = -1;
    int SopaIndex = -1;
    int MaterialIndex = -1;

    BellekYonetim _BellekYonetim = new BellekYonetim();
    VeriYonetimi _VeriYonetim = new VeriYonetimi();
    [Header("-------GENEL VERİLER")]
    public Animator Kaydedildi_Animator;
    public AudioSource[] Sesler;
    public List<ItemBilgileri> _ItemBilgileri = new List<ItemBilgileri>();
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();
    public Text[] TextObjeleri;

    string SatinAlmaText;
    string ItemText;

    void Start()
    {
        PuanText.text = _BellekYonetim.VeriOku_i("Puan").ToString();
        _BellekYonetim.VeriKaydet_string("Dil", "EN");        
        _VeriYonetim.Load();
        _ItemBilgileri = _VeriYonetim.ListeyiAktar();

        DurumuKontrolEt(0, true);
        DurumuKontrolEt(1, true);
        DurumuKontrolEt(2, true);

        foreach (var item in Sesler)
        {
            item.volume = _BellekYonetim.VeriOku_f("MenuFx");
        }

        _VeriYonetim.Dil_Load();
        _DilOkunanVeriler = _VeriYonetim.DilVerileriListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[1]);
        DilTercihiYonetimi();
    }
    void DilTercihiYonetimi()
    {

        if (_BellekYonetim.VeriOku_s("Dil") == "TR")
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_TR[i].Metin;
            }

            SatinAlmaText = _DilVerileriAnaObje[0]._DilVerileri_TR[5].Metin;
            ItemText = _DilVerileriAnaObje[0]._DilVerileri_TR[4].Metin;

        }
        else
        {
            for (int i = 0; i < TextObjeleri.Length; i++)
            {
                TextObjeleri[i].text = _DilVerileriAnaObje[0]._DilVerileri_EN[i].Metin;
            }
            SatinAlmaText = _DilVerileriAnaObje[0]._DilVerileri_EN[5].Metin;
            ItemText = _DilVerileriAnaObje[0]._DilVerileri_EN[4].Metin;
        }

    }

    void DurumuKontrolEt(int Bolum,bool islem=false)
    {
       
        if (Bolum==0)
        {
            #region
            if (_BellekYonetim.VeriOku_i("AktifSapka") == -1)
            {
                foreach (var item in Sapkalar)
                {
                    item.SetActive(false);
                }

                TextObjeleri[5].text = SatinAlmaText;                
                islemButonlari[0].interactable = false;
                islemButonlari[1].interactable = false;

                if (!islem)
                {
                    SapkaIndex = -1;
                    SapkaText.text = ItemText;
                }              

            }
            else
            {

                foreach (var item in Sapkalar)
                {
                    item.SetActive(false);
                }
                SapkaIndex = _BellekYonetim.VeriOku_i("AktifSapka");
                Sapkalar[SapkaIndex].SetActive(true);

                SapkaText.text = _ItemBilgileri[SapkaIndex].Item_Ad;
                TextObjeleri[5].text =SatinAlmaText;
                islemButonlari[0].interactable = false;
                islemButonlari[1].interactable = true;

            }
            #endregion

        }
        else if (Bolum == 1)
        {
            #region
            if (_BellekYonetim.VeriOku_i("AktifSopa") == -1)
            {

                foreach (var item in Sopalar)
                {
                    item.SetActive(false);
                }
                islemButonlari[0].interactable = false;
                islemButonlari[1].interactable = false;
                TextObjeleri[5].text =SatinAlmaText;
                if (!islem)
                {
                    SopaIndex = -1;
                    SopaText.text = ItemText;
                }         

            }
            else
            {
                foreach (var item in Sopalar)
                {
                    item.SetActive(false);
                }
                SopaIndex = _BellekYonetim.VeriOku_i("AktifSopa");
                Sopalar[SopaIndex].SetActive(true);

                SopaText.text = _ItemBilgileri[SopaIndex + 3].Item_Ad;
                TextObjeleri[5].text =SatinAlmaText;
                islemButonlari[0].interactable = false;
                islemButonlari[1].interactable = true;
            }
            #endregion

        }else
        {
            if (_BellekYonetim.VeriOku_i("AktifTema") == -1)
            {

                if (!islem)
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    MaterialIndex = -1;
                    MaterialText.text = ItemText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = false;
                }
                else
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = VarsayilanTema;
                    _Renderer.materials = mats;
                    TextObjeleri[5].text =SatinAlmaText;
                }               
            }
            else
            {
                MaterialIndex = _BellekYonetim.VeriOku_i("AktifTema");
                Material[] mats = _Renderer.materials;
                mats[0] = Materyaller[MaterialIndex];
                _Renderer.materials = mats;

                MaterialText.text = _ItemBilgileri[MaterialIndex + 6].Item_Ad;
                TextObjeleri[5].text =SatinAlmaText;
                islemButonlari[0].interactable = false;
                islemButonlari[1].interactable = true;
            }

        }      

    }   
    public void SatinAl()
    {
        Sesler[1].Play();
        if (AktifislemPaneliIndex!=-1)
        {
            switch(AktifislemPaneliIndex)
            {
                case 0:
                    SatinAlmaSonuc(SapkaIndex); 
                    break;
                case 1:
                    int Index = SopaIndex + 3;  
                    SatinAlmaSonuc(Index);                  
                    break;
                case 2:
                    int Index2 = MaterialIndex + 6;
                    SatinAlmaSonuc(Index2);
                    break;
            }
        }       

    }
    public void Kaydet()
    {
        Sesler[2].Play();
        if (AktifislemPaneliIndex != -1)
        {
            switch (AktifislemPaneliIndex)
            {
                case 0:
                    KaydetmeSonuc("AktifSapka", SapkaIndex);
                    break;
                case 1:
                    KaydetmeSonuc("AktifSopa", SopaIndex);
                    break;
                case 2:
                    KaydetmeSonuc("AktifTema", MaterialIndex);
                    break;
            }
        }
    }
    public void Sapka_Yonbutonlari(string islem)
    {
        Sesler[0].Play();
        if (islem=="ileri")
        {
            if (SapkaIndex==-1)
            {
                SapkaIndex = 0;
                Sapkalar[SapkaIndex].SetActive(true);
                SapkaText.text=_ItemBilgileri[SapkaIndex].Item_Ad;

                if (!_ItemBilgileri[SapkaIndex].SatinAlmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SapkaIndex].Puan + " - " + SatinAlmaText;
                    islemButonlari[1].interactable = false;

                    if (_BellekYonetim.VeriOku_i("Puan")< _ItemBilgileri[SapkaIndex].Puan)
                        islemButonlari[0].interactable = false;
                    else
                        islemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = true;
                }
            }
            else
            {
                Sapkalar[SapkaIndex].SetActive(false);
                SapkaIndex++;
                Sapkalar[SapkaIndex].SetActive(true);
                SapkaText.text = _ItemBilgileri[SapkaIndex].Item_Ad;

                if (!_ItemBilgileri[SapkaIndex].SatinAlmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SapkaIndex].Puan + " - " + SatinAlmaText;                   
                    islemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SapkaIndex].Puan)
                        islemButonlari[0].interactable = false;
                    else
                        islemButonlari[0].interactable = true;

                }
                else
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = true;
                }
            }

            //-----------------------------------------------

            if (SapkaIndex == Sapkalar.Length - 1)
                SapkaButonlari[1].interactable = false;
            else
                SapkaButonlari[1].interactable = true;

            if (SapkaIndex!=-1)
                SapkaButonlari[0].interactable = true;

        }
        else
        {
            if (SapkaIndex!=-1)
            {
                Sapkalar[SapkaIndex].SetActive(false);
                SapkaIndex--;
                if (SapkaIndex!=-1)
                {
                    Sapkalar[SapkaIndex].SetActive(true);
                    SapkaButonlari[0].interactable = true;
                    SapkaText.text = _ItemBilgileri[SapkaIndex].Item_Ad;
                    if (!_ItemBilgileri[SapkaIndex].SatinAlmaDurumu)
                    {
                        TextObjeleri[5].text = _ItemBilgileri[SapkaIndex].Puan + " - " + SatinAlmaText;
                        islemButonlari[1].interactable = false;
                        if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SapkaIndex].Puan)
                            islemButonlari[0].interactable = false;
                        else
                            islemButonlari[0].interactable = true;

                    }
                    else
                    {
                        TextObjeleri[5].text =SatinAlmaText;
                        islemButonlari[0].interactable = false;
                        islemButonlari[1].interactable = true;
                    }

                }
                else
                {
                    SapkaButonlari[0].interactable = false;
                    SapkaText.text = ItemText;
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                }

            }else
            {
                SapkaButonlari[0].interactable = false;
                SapkaText.text = ItemText;
                TextObjeleri[5].text =SatinAlmaText;
                islemButonlari[0].interactable = false;

            }
            //-----------------------------------------------
            if (SapkaIndex != Sapkalar.Length - 1)
                SapkaButonlari[1].interactable = true;
        }

    }
    public void Sopa_Yonbutonlari(string islem)
    {
        Sesler[0].Play();
        if (islem == "ileri")
        {
            if (SopaIndex == -1)
            {
                SopaIndex = 0;
                Sopalar[SopaIndex].SetActive(true);
                SopaText.text = _ItemBilgileri[SopaIndex + 3].Item_Ad;


                if (!_ItemBilgileri[SopaIndex + 3].SatinAlmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SopaIndex + 3].Puan + " - " + SatinAlmaText;

                    islemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SopaIndex + 3].Puan)
                        islemButonlari[0].interactable = false;
                    else
                        islemButonlari[0].interactable = true;

                }
                else
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = true;
                }

            }
            else
            {
                Sopalar[SopaIndex].SetActive(false);
                SopaIndex++;
                Sopalar[SopaIndex].SetActive(true);
                SopaText.text = _ItemBilgileri[SopaIndex + 3].Item_Ad;

                if (!_ItemBilgileri[SopaIndex + 3].SatinAlmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[SopaIndex + 3].Puan + " - " + SatinAlmaText;
                    islemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SopaIndex + 3].Puan)
                        islemButonlari[0].interactable = false;
                    else
                        islemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = true;
                }
            }

            //-----------------------------------------------

            if (SopaIndex == Sopalar.Length - 1)
                SopaButonlari[1].interactable = false;
            else
                SopaButonlari[1].interactable = true;

            if (SopaIndex != -1)
                SopaButonlari[0].interactable = true;

        }
        else
        {
            if (SopaIndex != -1)
            {
                Sopalar[SopaIndex].SetActive(false);
                SopaIndex--;
                if (SopaIndex != -1)
                {
                    Sopalar[SopaIndex].SetActive(true);
                    SopaButonlari[0].interactable = true;
                    SopaText.text = _ItemBilgileri[SopaIndex + 3].Item_Ad;

                    if (!_ItemBilgileri[SopaIndex + 3].SatinAlmaDurumu)
                    {
                        TextObjeleri[5].text = _ItemBilgileri[SopaIndex + 3].Puan + " - " + SatinAlmaText;
                        islemButonlari[1].interactable = false;
                        if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[SopaIndex + 3].Puan)
                            islemButonlari[0].interactable = false;
                        else
                            islemButonlari[0].interactable = true;
                    }
                    else
                    {
                        TextObjeleri[5].text =SatinAlmaText;
                        islemButonlari[0].interactable = false;
                        islemButonlari[1].interactable = true;
                    }
                }
                else
                {
                    SopaButonlari[0].interactable = false;
                    SopaText.text = ItemText;
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                }

            }
            else
            {
                SopaButonlari[0].interactable = false;
                SopaText.text = ItemText;
                TextObjeleri[5].text =SatinAlmaText;
                islemButonlari[0].interactable = false;

            }
            //-----------------------------------------------
            if (SopaIndex != Sopalar.Length - 1)
                SopaButonlari[1].interactable = true;
        }

    }
    public void Material_Yonbutonlari(string islem)
    {
        Sesler[0].Play();
        if (islem == "ileri")
        {
            if (MaterialIndex == -1)
            {
                MaterialIndex = 0;
                Material[] mats = _Renderer.materials;
                mats[0] = Materyaller[MaterialIndex];
                _Renderer.materials = mats;
                
                MaterialText.text = _ItemBilgileri[MaterialIndex + 6].Item_Ad;

                if (!_ItemBilgileri[MaterialIndex + 6].SatinAlmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[MaterialIndex + 6].Puan + " - " + SatinAlmaText;
                    islemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[MaterialIndex + 6].Puan)
                        islemButonlari[0].interactable = false;
                    else
                        islemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = true;
                }
            }
            else
            {
                
                MaterialIndex++;
                Material[] mats = _Renderer.materials;
                mats[0] = Materyaller[MaterialIndex];
                _Renderer.materials = mats;

                MaterialText.text = _ItemBilgileri[MaterialIndex + 6].Item_Ad;
                if (!_ItemBilgileri[MaterialIndex + 6].SatinAlmaDurumu)
                {
                    TextObjeleri[5].text = _ItemBilgileri[MaterialIndex + 6].Puan + " - " + SatinAlmaText;
                    islemButonlari[1].interactable = false;
                    if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[MaterialIndex + 6].Puan)
                        islemButonlari[0].interactable = false;
                    else
                        islemButonlari[0].interactable = true;
                }
                else
                {
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                    islemButonlari[1].interactable = true;
                }
            }

            //-----------------------------------------------

            if (MaterialIndex == Materyaller.Length - 1)
                MateryalButonlari[1].interactable = false;
            else
                MateryalButonlari[1].interactable = true;

            if (MaterialIndex != -1)
                MateryalButonlari[0].interactable = true;

        }
        else
        {
            if (MaterialIndex != -1)
            {
               
                MaterialIndex--;
                if (MaterialIndex != -1)
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = Materyaller[MaterialIndex];
                    _Renderer.materials = mats;

                    MateryalButonlari[0].interactable = true;
                    MaterialText.text = _ItemBilgileri[MaterialIndex + 6].Item_Ad;
                    if (!_ItemBilgileri[MaterialIndex + 6].SatinAlmaDurumu)
                    {
                        TextObjeleri[5].text = _ItemBilgileri[MaterialIndex + 6].Puan + " - " + SatinAlmaText;
                        islemButonlari[1].interactable = false;
                        if (_BellekYonetim.VeriOku_i("Puan") < _ItemBilgileri[MaterialIndex + 6].Puan)
                            islemButonlari[0].interactable = false;
                        else
                            islemButonlari[0].interactable = true;
                    }
                    else
                    {
                        TextObjeleri[5].text =SatinAlmaText;
                        islemButonlari[0].interactable = false;
                        islemButonlari[1].interactable = true;
                    }
                }
                else
                {
                    Material[] mats = _Renderer.materials;
                    mats[0] = VarsayilanTema;
                    _Renderer.materials = mats;
                    MateryalButonlari[0].interactable = false;
                    MaterialText.text = ItemText;
                    TextObjeleri[5].text =SatinAlmaText;
                    islemButonlari[0].interactable = false;
                }

            }
            else
            {
                Material[] mats = _Renderer.materials;
                mats[0] = VarsayilanTema;
                _Renderer.materials = mats;
                
                MateryalButonlari[0].interactable = false;
                MaterialText.text = ItemText;
                TextObjeleri[5].text =SatinAlmaText;
                islemButonlari[0].interactable = false;

            }
            //-----------------------------------------------
            if (MaterialIndex != Materyaller.Length - 1)
                MateryalButonlari[1].interactable = true;
        }

    }
    public void islemPaneliCikart(int Index)
    {
        Sesler[0].Play();
        DurumuKontrolEt(Index);
        GenelPaneller[0].SetActive(true);
        AktifislemPaneliIndex = Index;
        islemPanelleri[Index].SetActive(true);
        GenelPaneller[1].SetActive(true);
        islemCanvasi.SetActive(false);        
        
    }
    public void GeriDon()
    {
        Sesler[0].Play();
        GenelPaneller[0].SetActive(false);
        islemCanvasi.SetActive(true);
        GenelPaneller[1].SetActive(false);
        islemPanelleri[AktifislemPaneliIndex].SetActive(false);
        DurumuKontrolEt(AktifislemPaneliIndex,true);
        AktifislemPaneliIndex = -1;        
    }
    public void AnaMenuyeDon()
    {
        Sesler[0].Play();
        _VeriYonetim.Save(_ItemBilgileri);
        SceneManager.LoadScene(0);
       
    }

    //------------------------
    void SatinAlmaSonuc(int Index)
    {
        _ItemBilgileri[Index].SatinAlmaDurumu = true;
        _BellekYonetim.VeriKaydet_int("Puan", _BellekYonetim.VeriOku_i("Puan") - _ItemBilgileri[Index].Puan);
        TextObjeleri[5].text =SatinAlmaText;
        islemButonlari[0].interactable = false;
        islemButonlari[1].interactable = true;
        PuanText.text = _BellekYonetim.VeriOku_i("Puan").ToString();

    }
    void KaydetmeSonuc(string key, int Index)
    {
        _BellekYonetim.VeriKaydet_int(key, Index);
        islemButonlari[1].interactable = false;
        if (!Kaydedildi_Animator.GetBool("ok"))
            Kaydedildi_Animator.SetBool("ok", true);
    }

}
