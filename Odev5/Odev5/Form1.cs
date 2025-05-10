using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Odev5
{
    public partial class Form1 : Form
    {
        String giris;
        String sonGiris;

        Pen Kalem = new Pen(Color.Black, 2.0f);

        List<PointF> sinNok = new List<PointF>();
        List<PointF> cosNok = new List<PointF>();
        public Form1()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e) //Karekök
        {
            try
            {
                sonGiris = giris;
                double girilenSayi = Double.Parse(textBox1.Text);
                if(girilenSayi < 0)
                {
                    hataliGiris(" olmaz");
                    return;

                }
                string text = String.Format("{0:F3}", Calculate.KarekokBul(girilenSayi));
                textBox1.Text = text;
            }
            catch (Exception ex)
            {
                hataliGiris(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // inputu tutar
        {
            giris = textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e) //Mutlak Değer
        {
            try
            {
                sonGiris = giris;
                double girilenSayi = Double.Parse(textBox1.Text);
                textBox1.Text = Calculate.MutlakBul(girilenSayi) + "";
            }
            catch (Exception ex)
            {
                hataliGiris(ex.Message);
            }
        }
        private void hataliGiris(string hata)
        {
            textBox1.Text = "Hatalı girdin! " + hata;
        }

        private void button1_Click(object sender, EventArgs e)  //En büyük elemanı bulur
        {
            try
            {
                sonGiris = giris;
                string[] tokens = giris.Split(',');
                double[] sayilar = new double[tokens.Length];
                for (int i = 0; i < tokens.Length; i++)
                    sayilar[i] = Double.Parse(tokens[i]);

                double enBuyuk = Calculate.EnbBul(sayilar);
                textBox1.Text = enBuyuk + "";
            }
            catch (Exception ex)
            {
                hataliGiris(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)  
        {
            {
                textBox1.Text = sonGiris; // geri alır
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                sonGiris = giris;
                double aci = Double.Parse(giris);
                double x = Calculate.SinBul(aci);
                string text = String.Format("{0:F3}", x);
                textBox1.Text = text;
            }
            catch (Exception ex)
            {
                hataliGiris(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                sonGiris = giris;
                double aci = Double.Parse(giris);
                double x = Calculate.CosBul(aci);
                string text = String.Format("{0:F2}", x);
                textBox1.Text = text;
            }
            catch (Exception ex)
            {
                hataliGiris(ex.Message);
            }
        }
        private void noktalariHesapla(float bas, float son) //nokta hesapla
        {
            sinNok.Clear();
            cosNok.Clear();
            float xCizgisi = Ciz.Width / 2 + bas;
            for (float aci = bas; aci <= son; aci++)
            {
                float sinY = (float)Calculate.SinBul(aci) * -1;
                float cosY = (float)Calculate.CosBul(aci) * -1;

                // Noktalari büyütür
                float buyutme = Ciz.Height / 2;


                sinNok.Add(new PointF(xCizgisi, sinY * buyutme + buyutme));
                cosNok.Add(new PointF(xCizgisi, cosY * buyutme + buyutme));
                xCizgisi++;
            }
        }

        private void button7_Click(object sender, EventArgs e) //grafik cizme
        {
            float baslangicDerece = float.Parse(textBox3.Text); // bas
            float bitisDerece = float.Parse(textBox2.Text); // son
            noktalariHesapla(baslangicDerece, bitisDerece); //girilen değerler arasındaki noktaları hesaplar

            Graphics g = Ciz.CreateGraphics();
            g.Clear(Color.White);
            g.DrawLine(Kalem, new PointF(Ciz.Width / 2, 0), new PointF(Ciz.Width / 2, Ciz.Height)); // Y ekseni
            g.DrawLine(Kalem, new PointF(0, Ciz.Height / 2), new PointF(Ciz.Width, Ciz.Height / 2)); // X Ekseni
            if (radioButton1.Checked) // sin
            {
                g.DrawCurve(Kalem, sinNok.ToArray()); // Grafiği çiziyor
            }
            else
            {
                g.DrawCurve(Kalem, cosNok.ToArray()); // Grafiği çiziyor
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

public class Calculate
{
   public static double KarekokBul(double sayi)
    {
        double tahmin = sayi / 2;
        for (int i = 0; i < 20; i++)
        {
            tahmin = (tahmin + sayi / tahmin) / 2;
        }
        return tahmin;
    }
    public static double EnbBul(double[] sayilar)
    {

        double enBuyuk = sayilar[0];
        for (int i = 1; i < sayilar.Length; i++)
        {
            if (sayilar[i] > enBuyuk)
            {
                enBuyuk = sayilar[i];
            }
        }
        return enBuyuk;
    }
    public static double MutlakBul(double sayi)
    {
        if (sayi < 0)
            return -sayi;
        else
            return sayi;
    }
    public static double SinBul(double x)
    {
        int terimSayisi = 10;
        double sinüs = 0;
        x = DereceToRadyan(x);
        for (int i = 0; i < terimSayisi; i++)
        {
            // Her terimi hesapla ve toplama ekle
            sinüs += (Us(-1, i) * Us(x, 2 * i + 1)) / Faktoriyel(2 * i + 1);
        }

        return sinüs;
    }
    static double Us(double sayi, int us)
    {
        if (us == 0){
            return 1;
        }
        double sonuc = sayi;
        for (int i = 1;i< us; i++)
        {
            sonuc *= sayi;
        }
        return sonuc;
    }

    static double Faktoriyel(int n)
    {
        // Faktöriyel hesapla
        double faktoriyel = 1;
        for (int i = 2; i <= n; i++)
        {
            faktoriyel *= i;
        }
        return faktoriyel;
    }
    static double DereceToRadyan(double derece)
    {
        return derece * 3.14 / 180.0;
    }

    public static double CosBul(double x)
    {
        x = DereceToRadyan(x);
        int terimSayisi = 10;
        double cosinus = 0;

        for (int i = 0; i < terimSayisi; i++)
        {
            // terimleri bulup toplama ekle
            cosinus += (Us(-1, i) * Us(x, 2 * i)) / Faktoriyel(2 * i);
        }

        return cosinus;
    }
}

