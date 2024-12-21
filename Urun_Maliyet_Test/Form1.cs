using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Urun_Maliyet_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection bgl = new SqlConnection(@"Data Source=DESKTOP-F1A12T8\KORAY;Initial Catalog=Test_Maliyet;Integrated Security=True");

        void malzemeliste()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLMALZEMELER", bgl);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void urunlistesi()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select * From TBLURUNLER", bgl);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        }

        void kasa()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("Select * From TBLKASA", bgl);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }

        void urunler()
        {
            bgl.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLURUNLER", bgl);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbUrun.ValueMember = "URUNID";
            CmbUrun.DisplayMember = "AD";
            CmbUrun.DataSource = dt;
            bgl.Close();
        }

        void malzemeler()
        {
            bgl.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLMALZEMELER", bgl);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbMalzeme.ValueMember = "MALZEMEID";
            CmbMalzeme.DisplayMember = "AD";
            CmbMalzeme.DataSource = dt;
            bgl.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            malzemeliste();

            urunler();

            malzemeler();
        }

        private void BtnUrunlList_Click(object sender, EventArgs e)
        {
            urunlistesi();
        }

        private void BtnMalzemeList_Click(object sender, EventArgs e)
        {
            malzemeliste();
        }

        private void BtnKasa_Click(object sender, EventArgs e)
        {
            kasa();
        }

        private void BtnMalzemeEkle_Click(object sender, EventArgs e)
        {
            try
            {
                bgl.Open();
                SqlCommand cmd = new SqlCommand("Insert Into TBLMALZEMELER (AD, STOK, FIYAT, NOTLAR) Values (@p1, @p2, @p3, @p4)", bgl);
                cmd.Parameters.AddWithValue("@p1", TxtMalzemeAd.Text);
                cmd.Parameters.AddWithValue("@p2", decimal.Parse(TxtMalzemeStok.Text));
                cmd.Parameters.AddWithValue("@p3", decimal.Parse(TxtMalzemeFiyat.Text));
                cmd.Parameters.AddWithValue("@p4", TxtMalzemeNot.Text);
                cmd.ExecuteNonQuery();
                bgl.Close();
                MessageBox.Show("Malzeme Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                malzemeliste();
            }
            catch
            {
                MessageBox.Show("Girilen Değerleri Kontrol Edin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            malzemeliste();
            
        }

        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            if (TxtUrunMFiyat.Text != "" && TxtUrunSFiyat.Text != "" && TxtUrunStok.Text != "")
            {
                bgl.Open();
                SqlCommand cmd = new SqlCommand("Insert Into TBLURUNLER (AD, MFIYAT, SFIYAT, STOK) Values (@p1, @p2, @p3, @p4)", bgl);
                cmd.Parameters.AddWithValue("@p1", TxtUrunAd.Text.ToUpper());
                cmd.Parameters.AddWithValue("@p2", decimal.Parse(TxtUrunMFiyat.Text));
                cmd.Parameters.AddWithValue("@p3", decimal.Parse(TxtUrunSFiyat.Text));
                cmd.Parameters.AddWithValue("@p4", Convert.ToInt32(TxtUrunStok.Text));
                cmd.ExecuteNonQuery();
                bgl.Close();
                MessageBox.Show("Ürün Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                bgl.Open();
                double giris, cikis;

                if (TxtUrunStok.Text == "")
                {
                    TxtUrunStok.Text = "0";
                }
                cikis = Convert.ToDouble(TxtUrunStok.Text) * Convert.ToDouble(TxtUrunMFiyat.Text);
                giris = Convert.ToDouble(TxtUrunStok.Text) * Convert.ToDouble(TxtUrunSFiyat.Text);
                TxtStokxMFiyat.Text = giris.ToString();
                TxtStokxSFiyat.Text = cikis.ToString();

                SqlCommand cmd2 = new SqlCommand("Insert Into TBLKASA (GIRIS, CIKIS) Values (@p5, @p6)", bgl);
                cmd2.Parameters.AddWithValue("@p5", decimal.Parse(TxtStokxSFiyat.Text));
                cmd2.Parameters.AddWithValue("@p6", decimal.Parse(TxtStokxMFiyat.Text));
                cmd2.ExecuteNonQuery();
                bgl.Close();
                MessageBox.Show("Kasaya İşlem Girildi", "Kasa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bgl.Close();
            }
            else
            {
                bgl.Open();
                SqlCommand cmd = new SqlCommand("Insert Into TBLURUNLER (AD) Values (@p1)", bgl);
                cmd.Parameters.AddWithValue("@p1", TxtUrunAd.Text.ToUpper());
                cmd.ExecuteNonQuery();
                bgl.Close();
            }
            urunlistesi();
            urunler();
        }

        private void BtnUrunOlustur_Click(object sender, EventArgs e)
        {
            bgl.Open();
            SqlCommand cmd = new SqlCommand("Insert Into TBLFIRIN (URUNID, MALZEMEID, MIKTAR, MALIYET) Values (@p1, @p2, @p3, @p4)", bgl);
            cmd.Parameters.AddWithValue("@p1", CmbUrun.SelectedValue);
            cmd.Parameters.AddWithValue("@p2", CmbMalzeme.SelectedValue);
            cmd.Parameters.AddWithValue("@p3", decimal.Parse(TxtMiktar.Text));
            cmd.Parameters.AddWithValue("@p4", decimal.Parse(TxtMaliyet.Text));
            cmd.ExecuteNonQuery();
            bgl.Close();
            MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listBox1.Items.Add(CmbMalzeme.Text + " - " + TxtMaliyet.Text);
            TxtMiktar.Text = "";
            TxtMaliyet.Text = "";
        }

        private void TxtMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;

            if (TxtMiktar.Text == "")
            {
                TxtMiktar.Text = "0";
            }
            bgl.Open();
            SqlCommand cmd = new SqlCommand("Select * From TBLMALZEMELER Where MALZEMEID = @p1", bgl);
            cmd.Parameters.AddWithValue("@p1", CmbMalzeme.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TxtMaliyet.Text = dr[3].ToString();
            }
            bgl.Close();

            maliyet = Convert.ToDouble(TxtMaliyet.Text) / 1000 * Convert.ToDouble(TxtMiktar.Text);

            TxtMaliyet.Text = maliyet.ToString();
        }

        private void CmbMalzeme_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            double satis, urunmaliyet;
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtUrunID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

            bgl.Open();
            SqlCommand cmd = new SqlCommand("Select Sum(MALIYET) From TBLFIRIN Where URUNID = @p1", bgl);
            cmd.Parameters.AddWithValue("@p1", TxtUrunID.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunMFiyat.Text = dr[0].ToString();
            }
            bgl.Close();

            if (TxtUrunMFiyat.Text != "")
            {
                urunmaliyet = Convert.ToDouble(TxtUrunMFiyat.Text);
                satis = (urunmaliyet / 100) * 100 + urunmaliyet;
                TxtUrunSFiyat.Text = satis.ToString();
            }
        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnUrunGuncelle_Click(object sender, EventArgs e)
        {
            bgl.Open();
            SqlCommand cmd = new SqlCommand("Update TBLURUNLER Set AD = @p1, MFIYAT = @p2, SFIYAT = @p3, STOK = @p4 Where URUNID = @p5", bgl);
            cmd.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
            cmd.Parameters.AddWithValue("@p2", decimal.Parse(TxtUrunMFiyat.Text));
            cmd.Parameters.AddWithValue("@p3", decimal.Parse(TxtUrunSFiyat.Text));
            cmd.Parameters.AddWithValue("@p4", TxtUrunStok.Text);
            cmd.Parameters.AddWithValue("@p5", TxtUrunID.Text);
            cmd.ExecuteNonQuery();
            bgl.Close();
            MessageBox.Show("Ürün Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            urunlistesi();            
        }

        private void TxtUrunStok_TextChanged(object sender, EventArgs e)
        {
            //double maliyet, satis;

            //if (TxtUrunStok.Text == "")
            //{
            //    TxtUrunStok.Text = "0";
            //}
            //maliyet = Convert.ToDouble(TxtUrunStok.Text) * Convert.ToDouble(TxtUrunMFiyat.Text);
            //satis = Convert.ToDouble(TxtUrunStok.Text) * Convert.ToDouble(TxtUrunSFiyat.Text);
            //TxtStokxMFiyat.Text = maliyet.ToString();
            //TxtStokxSFiyat.Text = satis.ToString();

        }
    }
}
