using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Olcay;
public class Ayarlar_Manager : MonoBehaviour
{
    public AudioSource ButonSes;
    [Header("----------SLİDER OBJELERİ")]
    public Slider MenuSes;
    public Slider MenuFx;
    public Slider OyunSes; 

    BellekYonetim _BellekYonetim = new BellekYonetim();
    VeriYonetimi _VeriYonetim = new VeriYonetimi();
    List<DilVerileriAnaObje> _DilOkunanVeriler = new List<DilVerileriAnaObje>();   

    [Header("----------DİL TERCİHİ OBJELERİ")]
    public TextMeshProUGUI DilText;
    public Button[] Dilbutonlari;
    public List<DilVerileriAnaObje> _DilVerileriAnaObje = new List<DilVerileriAnaObje>();
    public TextMeshProUGUI[] TextObjeleri;
#pragma warning disable IDE0052 // Okunmamış özel üyeleri kaldır
    int AktifDilIndex;
#pragma warning restore IDE0052 // Okunmamış özel üyeleri kaldır

    void Start()
    {
        ButonSes.volume = _BellekYonetim.VeriOku_f("MenuFx");

        MenuSes.value = _BellekYonetim.VeriOku_f("MenuSes");
        MenuFx.value = _BellekYonetim.VeriOku_f("MenuFx");
        OyunSes.value = _BellekYonetim.VeriOku_f("OyunSes");     

        _VeriYonetim.Dil_Load();
        _DilOkunanVeriler = _VeriYonetim.DilVerileriListeyiAktar();
        _DilVerileriAnaObje.Add(_DilOkunanVeriler[4]);
        DilTercihiYonetimi();
        DilDurumunuKontrolEt();
    }
    void DilTercihiYonetimi()
    {

        if (_BellekYonetim.VeriOku_s("Dil") == "TR")
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
    public void SesAyarla(string HangiAyar)
    {

        switch(HangiAyar)
        {

            case "menuses":               
                 _BellekYonetim.VeriKaydet_float("MenuSes", MenuSes.value);
                break;

            case "menufx":               
                _BellekYonetim.VeriKaydet_float("MenuFx", MenuFx.value);
                break;

            case "oyunses":               
                _BellekYonetim.VeriKaydet_float("OyunSes", OyunSes.value);
                break;

        }      

    }
    public void GeriDon()
    {
        ButonSes.Play();
        SceneManager.LoadScene(0);
    }

    void DilDurumunuKontrolEt()
    {

        if (_BellekYonetim.VeriOku_s("Dil")=="TR")
        {
            AktifDilIndex = 0;
            DilText.text = "TÜRKÇE";
            Dilbutonlari[0].interactable = false;
        }
        else
        {
            AktifDilIndex = 1;
            DilText.text = "ENGLISH";
            Dilbutonlari[1].interactable = false;
        }
    }
    public void DilDegistir(string Yon)
    {

        if(Yon=="ileri")
        {
            AktifDilIndex = 1;
            DilText.text = "ENGLISH";
            Dilbutonlari[1].interactable = false;
            Dilbutonlari[0].interactable = true;
            _BellekYonetim.VeriKaydet_string("Dil", "EN");
            DilTercihiYonetimi();
        }
        else
        {
            AktifDilIndex = 0;
            DilText.text = "TÜRKÇE";
            Dilbutonlari[0].interactable = false;
            Dilbutonlari[1].interactable = true;
            _BellekYonetim.VeriKaydet_string("Dil", "TR");
            DilTercihiYonetimi();
        }

        ButonSes.Play();
    }
}
