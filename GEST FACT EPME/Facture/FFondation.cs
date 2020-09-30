﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EPME.Facture
{
    public partial class FFondation : DevComponents.DotNetBar.Office2007Form
    {
        MetierClient cli=null;
        Boolean change = false;
        Boolean modif = false;
        private metier met = Program.met;
        public Boolean newf = true;
        public Boolean Retenue;
       public String NumFact = "";
       public String typecalc = "";
        public FFondation()
        {
            InitializeComponent();
        }

        private void Tcodc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
                Tnomc.Focus();
        }

        private void Tcodc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
            {
                if (Tcodc.Text != "")
                {
                    rechcli rcli = new rechcli("client", Tcodc.Text, null);
                    rcli.ShowDialog();
                    if (rcli.valider)
                    {
                        cli = new MetierClient(rcli.code);
                        change = true;
                        mygrid1.Enabled = true;
                        mygrid1.Focus();
                    }
                    if (cli != null)
                    {
                        Tcodc.Text = cli.Codec;
                        Tnomc.Text = cli.NomC;
                        Tadrc.Text = cli.adrc;
                        Retenue = cli.ExenorationClient;
                        mygrid1.Focus();
                    }
                }
            }
        }

        private void Tnomc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
            {
                mygrid1.Focus();
            }
        }

        private void Tnomc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
            {
                if (Tnomc.Text != "")
                {
                    rechcli rcli = new rechcli("client", null, Tnomc.Text);
                    rcli.ShowDialog();
                    if (rcli.valider)
                    {
                        cli = new MetierClient(rcli.code);
                        change = true;
                        mygrid1.Enabled = true;
                    }
                    if (cli != null)
                    {
                        Tcodc.Text = cli.Codec;
                        Tnomc.Text = cli.NomC;
                        Tadrc.Text = cli.adrc;
                        Retenue = cli.ExenorationClient;
                        mygrid1.Focus();
                    }
                }
            }
        }

        private void nfact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
                datefact.Focus();
        }

        private void datefact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
            {
                    tref.Focus();
            }
            
        }

        private void mygrid1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex!=-1)
                if (e.ColumnIndex == 3 || e.ColumnIndex == 4 ||  e.ColumnIndex == 7 || e.ColumnIndex == 9)
            {
                double qt1=0,qt2=0, qte = 0, puht = 0, tva=18,tht=0,ttc=0;
                double.TryParse(mygrid1.Rows[e.RowIndex].Cells["VBP"].Value + "", out qt1);
                double.TryParse(mygrid1.Rows[e.RowIndex].Cells["VB350"].Value + "", out qt2);
                qte = qt1 + qt2;
                double.TryParse(mygrid1.Rows[e.RowIndex].Cells["PUHTF"].Value + "", out puht);
                double.TryParse(mygrid1.Rows[e.RowIndex].Cells["TVAFF"].Value + "", out tva);
                if (mygrid1.Rows[e.RowIndex].Cells["TVAFF"].Value == null)
                    tva = 18;
                tht = qte * puht;
                ttc = tht * (1 + (tva / 100));
                mygrid1.Rows[e.RowIndex].Cells["VBP"].Value = qt1.ToString("N5");
                mygrid1.Rows[e.RowIndex].Cells["VB350"].Value = qt2.ToString("N3");
                mygrid1.Rows[e.RowIndex].Cells["VTOTAL"].Value = qte.ToString("N5");
                mygrid1.Rows[e.RowIndex].Cells["PUHTF"].Value = puht.ToString("N3");
                mygrid1.Rows[e.RowIndex].Cells["PTHT"].Value = tht.ToString("N3");
                mygrid1.Rows[e.RowIndex].Cells["TVAFF"].Value = tva.ToString("N2");
                mygrid1.Rows[e.RowIndex].Cells["TTCF"].Value = ttc.ToString("N3");
                calcultotal();
            }
        }


        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string test_sauve()
        {
            String error = "";
            if (cli == null)
            {
                Tcodc.Focus();
                error = "saisir Code Client";
            }
            if (mygrid1.Rows.Count == 1)
            {
                mygrid1.Focus();
                error = "Verifier Ligne Facture";
            }
            if (datefact.Text.Equals(""))
            {
                datefact.Focus();
                error = "saisir Date Facture";
            }
            foreach (DataGridViewRow dr in mygrid1.Rows)
            {
                if (!dr.IsNewRow)
                {
                    if ((dr.Cells["Npylon"].Value+"").Equals("") || dr.Cells["Npylon"].Value.Equals(null))
                    {
                        mygrid1.CurrentCell = dr.Cells["Npylon"];
                        mygrid1.Focus();
                        error = "Verifier Ligne Facture";
                        break;
                    }
                    if ((dr.Cells["TypePylone"].Value+"").Equals("") || dr.Cells["TypePylone"].Value.Equals(null))
                    {
                        mygrid1.CurrentCell = dr.Cells["TypePylone"];
                        mygrid1.Focus();
                        error = "Verifier Ligne Facture";
                        break;
                    }
                    if ((dr.Cells["TypeMassif"].Value+"").Equals("") || dr.Cells["TypeMassif"].Value.Equals(null))
                    {
                        mygrid1.CurrentCell = dr.Cells["TypeMassif"];
                        mygrid1.Focus();
                        error = "Verifier Ligne Facture";
                        break;
                    }
                    if ((dr.Cells["VBP"].Value+"").Equals("") || dr.Cells["VBP"].Value.Equals(null))
                    {
                        mygrid1.CurrentCell = dr.Cells["VBP"];
                        mygrid1.Focus();
                        error = "Verifier Ligne Facture";
                        break;
                    }
                    if ((dr.Cells["U"].Value+"").Equals("") || dr.Cells["U"].Value.Equals(null))
                    {
                        mygrid1.CurrentCell = dr.Cells["U"];
                        mygrid1.Focus();
                        error = "Verifier Ligne Facture";
                        break;
                    }
                    if ((dr.Cells["PUHTF"].Value+"").Equals("") || dr.Cells["PUHTF"].Value.Equals(null))
                    {
                        mygrid1.CurrentCell = dr.Cells["PUHTF"];
                        mygrid1.Focus();
                        error = "Verifier Ligne Facture";
                        break;
                    }
                }
            }
            return error;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (cli != null)
            {
                if (!modif)
                {
                    string err = test_sauve();
                    if (err.Equals(""))
                    {
                        SaveIncrementationNumero();
                        String dat = datefact.Value.ToString("yyyy-MM-dd");
                        string sql = "INSERT INTO Facture (NUMF,REF,NREF,VREF,CODEC, NOMC,ADRC,DATEF, CODES,codee, THT,RAVANCE,RGARANTIE,NETHT, TVA,TIMBRE,TTC,RS50,RS15,NET,MODEP,type,VGARANTIE,VAVANCE,prorata) VALUE ("
                            + "'" + nfact.Text + "',"
                            + "'" + met.CString(tref.Text) + "',"
                            + "'" + met.CString(tnref.Text) + "',"
                            + "'" + met.CString(tvref.Text) + "',"
                            + "'" + met.CString(Tcodc.Text) + "',"
                            + "'" +met.CString( Tnomc.Text) + "',"
                            + "'" +met.CString( Tadrc.Text )+ "',"
                            + "'" + dat + "',"
                            + "'" + Program.Societe + "',"
                            + "'" + Program.Exercice + "',"
                            + "'" + Ttht.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Tav.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Tret.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Tnetht.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Ttva.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Ttimbre.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Tttc.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + rs50.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + rs15.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + net.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + Modep.SelectedValue + "',"
                            + "'F',"
                            + "'" + Tpret.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'" + tpav.Text.Replace(Program.sep, string.Empty) + "',"
                             + "'" + tprorata.Text.Replace(Program.sep, string.Empty) + "'"
                            + ")";

                        met.Execute(sql);

                        //Fondation
                        String sqlF = "insert INTO lfondation (NPYLONE,TYPEPYLONE,Tmassif,VBETPROPRETE,VB350,VTOTAL,U,PUHT,PTHT,TVA,CODES,codee,NUMF) values";
                        Boolean frst = true;
                        foreach (DataGridViewRow dr in mygrid1.Rows)
                        {
                            if (!dr.IsNewRow)
                            {
                                if (!frst)
                                {
                                    sqlF += ",(";
                                }
                                else
                                {
                                    sqlF += "(";
                                }
                                sqlF += "'" + met.CString(dr.Cells["Npylon"].Value) + "'"
                                    + ",'" + met.CString(dr.Cells["TypePylone"].Value )+ "'"
                                    + ",'" +met.CString( dr.Cells["TypeMassif"].Value) + "'"
                                    + ",'" + (dr.Cells["VBP"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["VB350"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["VTOTAL"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + met.CString(dr.Cells["U"].Value) + "'"
                                    + ",'" + (dr.Cells["PUHTF"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["PTHT"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["TVAFF"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + Program.Societe + "'"
                                    + ",'" + Program.Exercice + "'"
                                    + ",'" + nfact.Text + "')";
                                frst = false;
                            }
                        }
                        if (!frst)
                        {
                            met.Execute(sqlF);
                        }
                        MessageBox.Show("Sauvgard avec sucée.");
                        buttonX2.Visible = false;
                        buttonX3.Visible = true ;
                    }
                    else
                        MessageBox.Show(err);
                }
                else
                {
                    string err = test_sauve();
                    if (err.Equals(""))
                    {
                        String dat = datefact.Value.ToString("yyyy-MM-dd");
                        string sql = "UPDATE Facture SET REF='" +met.CString( tref.Text)
                            + "',NREF='" + met.CString(tnref.Text)
                            + "',VREF='" + met.CString(tvref.Text)
                            + "',CODEC='" + met.CString(Tcodc.Text)
                            + "', NOMC='" + met.CString(Tnomc.Text)
                            + "',ADRC='" +met.CString( Tadrc.Text)
                            + "',DATEF='" + dat
                            + "', CODES='" + Program.Societe
                            + "', THT='" + Ttht.Text.Replace(Program.sep, string.Empty)
                            + "',RAVANCE='" + Tav.Text.Replace(Program.sep, string.Empty)
                            + "',RGARANTIE='" + Tret.Text.Replace(Program.sep, string.Empty)
                            + "',NETHT='" + Tnetht.Text.Replace(Program.sep, string.Empty)
                            + "', TVA='" + Ttva.Text.Replace(Program.sep, string.Empty)
                            + "',TIMBRE='" + Ttimbre.Text.Replace(Program.sep, string.Empty)
                            + "',TTC='" + Tttc.Text.Replace(Program.sep, string.Empty)
                            + "'RS50='" + rs50.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'RS15='" + rs15.Text.Replace(Program.sep, string.Empty) + "',"
                            + "'NET='" + net.Text.Replace(Program.sep, string.Empty) + "',"
                            + "',MODEP='" + Modep.SelectedValue
                            + "',type='F',VGARANTIE='" + Tpret.Text.Replace(Program.sep, string.Empty)
                            + "',VAVANCE='" + tpav.Text.Replace(Program.sep, string.Empty)
                            + "',prorata='" + tprorata.Text.Replace(Program.sep, string.Empty)
                            + "'WHERE codes='" + Program.Societe
                            + "' and codee='" + Program.Exercice
                            + "' and numf = '" + NumFact + "'";

                        met.Execute(sql);

                        //Fondation
                        met.Execute("DELETE  FROM lfondation WHERE codes='" + Program.Societe + "' and numf='" + NumFact + "'");
                        String sqlF = "INSERT INTO lfondation (NPYLONE,TYPEPYLONE,Tmassif,VBETPROPRETE,VB350,VTOTAL,U,PUHT,PTHT,TVA,CODES,CODEE,NUMF) values";
                        Boolean frst = true;
                        foreach (DataGridViewRow dr in mygrid1.Rows)
                        {
                            if (!dr.IsNewRow)
                            {
                                if (!frst)
                                {
                                    sqlF += ",(";
                                }
                                else
                                {
                                    sqlF += "(";
                                }
                                sqlF += "'" + met.CString(dr.Cells["Npylon"].Value )+ "'"
                                    + ",'" + met.CString(dr.Cells["TypePylone"].Value) + "'"
                                    + ",'" +met.CString( dr.Cells["TypeMassif"].Value )+ "'"
                                    + ",'" + (dr.Cells["VBP"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["VB350"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["VTOTAL"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" +met.CString( dr.Cells["U"].Value) + "'"
                                    + ",'" + (dr.Cells["PUHTF"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["PTHT"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + (dr.Cells["TVAFF"].Value + "").Replace(Program.sep, string.Empty) + "'"
                                    + ",'" + Program.Societe + "'"
                                    + ",'" + Program.Exercice + "'"
                                    + ",'" + nfact.Text + "')";
                                frst = false;
                            }
                        }
                        if (!frst)
                        {
                            met.Execute(sqlF);
                        }
                        MessageBox.Show("Sauvgard avec sucée.");
                        buttonX2.Visible = false;
                        buttonX3.Visible = true;
                    }
                    else
                        MessageBox.Show(err);
                }
            }
            else
                MessageBox.Show("Verifier Le Code Client.");
        }

        private void calcultotal()
        {
            double THT = 0, NETTHT = 0, TVA = 0, TTC = 0, Timbre = 0, prorata = 0, brut = 0, trs50 = 0, trs15 = 0, tnet = 0;
            double.TryParse(Ttimbre.Text, out Timbre);
            double.TryParse(tprorata.Text, out prorata);
            Double pav = 0, pret = 0;
            double.TryParse(tpav.Text, out pav);
            double.TryParse(Tpret.Text, out pret);
            double av = 0;
            double ret = 0;
            if (prorata == 0)
                prorata = 100;
            foreach (DataGridViewRow dr in mygrid1.Rows)
            {
                double xtht = 18, xtva = 0, xttc = 0;
                double.TryParse(dr.Cells["PTHT"].Value + "", out xtht);
                double.TryParse(dr.Cells["TVAFF"].Value + "", out xtva);
                double.TryParse(dr.Cells["TTCF"].Value + "", out xttc);
                brut += xtht;
                xtht = xtht * prorata / 100;
                double tav = xtht * pav / 100;
                double tret = xtht * pret / 100;
                av += tav;
                ret += tret;
                THT += xtht;
                if (typecalc == "NHT")
                {
                    xtht = xtht - tav - tret;
                    NETTHT += xtht;
                }
                else
                {
                    NETTHT += xtht - tav - tret;
                }

                TVA += (xtht * xtva / 100);
            }

            if (av != 0)
                Tav.Text = av.ToString("N3");
            else
                Tav.Text = "";

            if (ret != 0)
                Tret.Text = ret.ToString("N3");
            else
                Tret.Text = "";

            Tnetht.Text = NETTHT.ToString("N3");
            Ttva.Text = TVA.ToString("N3");
            TTC = NETTHT + TVA;
            Tttc.Text = TTC.ToString("N3");

            if (Retenue)
            {
                trs50 = TVA * 50 / 100;
                trs15 = TTC * 1.5 / 100;
                rs50.Text = trs50.ToString("N3");
                rs15.Text = trs15.ToString("N3");
            }
            else
            {
                rs50.Text = 0.ToString("N3");
                rs15.Text = 0.ToString("N3");
            }



            tnet = TTC + Timbre - trs50 - trs15;
            net.Text = tnet.ToString("N3");

            Ttht.Text = THT.ToString("N3");
            tbrut.Text = brut.ToString("N3");
        }

        private void textBoxX18_TextChanged(object sender, EventArgs e)
        {
            calcultotal();
        }

        private void FFondation_Load(object sender, EventArgs e)
        {
            String sqlmp = "Select * From modalite where codes='"+Program.Societe+"'";
           DataSet dsm= met.recuperer_table(sqlmp,"modalite");
           BindingSource bs = new BindingSource(dsm, "modalite");
           Modep.DisplayMember = "libelle";
           Modep.ValueMember = "Code";
           Modep.DataSource = bs;

           rbnht.Checked = true;
           rbht.Checked = false;

           if (newf)
           {
               increment();
               nfact.Text = NumFact;
           }
           else
           {
               modif = true;
               string sql = "select * from facture where codes='" + Program.Societe + "' and codee='" + Program.Exercice + "' and numf = '" + NumFact + "'";
               DataSet ds = met.recuperer_table(sql);
               DataRow drf=null;
               try{
                    drf= ds.Tables[0].Rows[0];
               }catch{}
               if (drf != null)
               {
                   nfact.ReadOnly = true;
                   
                       try
                   {
                       datefact.Value =  drf.Field<DateTime>("DATEF");
                   }
                   catch { }
                   nfact.Text = drf["NUMF"]+"";
                   tref.Text = drf["REF"] + "";
                   tvref.Text = drf["VREF"] + "";
                   tnref.Text = drf["NREF"] + "";
                   Tcodc.Text = drf["CODEC"] + "";
                   cli = new MetierClient(Tcodc.Text);
                   Tnomc.Text = drf["NOMC"] + "";
                   Tadrc.Text = drf["ADRC"] + "";
                   Modep.SelectedValue = drf["MODEP"] + ""; 
                   try
                   {
                       Ttht.Text = drf.Field<decimal>("THT").ToString("N3") + "";
                   }
                   catch { }
                   try
                   {
                   Tret.Text = drf.Field<decimal>("RGARANTIE").ToString("N3") + "";
                        }
                   catch { }
                   try
                   {
                   Tav.Text = drf.Field<decimal>("RAVANCE").ToString("N3") + "";
                        }
                   catch { }
                   try
                   {
                   Ttimbre.Text = drf.Field<decimal>("TIMBRE").ToString("N3") + "";
                        }
                   catch { }
                   try
                   {
                   Ttva.Text = drf.Field<decimal>("TVA").ToString("N3") + "";
                        }
                   catch { }
                   try
                   {
                   Tttc.Text = drf.Field<decimal>("TTC").ToString("N3") + "";
                        }
                   catch { }
                   try
                   {
                   Tnetht.Text = drf.Field<decimal>("NETHT").ToString("N3") + "";
                        }
                   catch { }
                   try
                   {
                   tpav.Text = drf.Field<decimal>("VAVANCE").ToString("N2") + "";
                        }
                   catch { }
                   try
                   {
                   Tpret.Text = drf.Field<decimal>("VGARANTIE").ToString("N2") + "";
                        }
                   catch { }
               }
               string sqld = "select * from lfondation where codes = '" + Program.Societe + "' and codee='" + Program.Exercice + "' and numf = '" + NumFact + "'";
               DataSet dsd = met.recuperer_table(sqld);
               try {
                   int i = 0;
                   foreach (DataRow dr in dsd.Tables[0].Rows)
                   {
                       mygrid1.Rows.Add();
                       mygrid1.Rows[i].Cells["Npylon"].Value = dr["NPYLONE"];
                       mygrid1.Rows[i].Cells["TypePylone"].Value = dr["TYPEPYLONE"];
                       mygrid1.Rows[i].Cells["TypeMassif"].Value = dr["Tmassif"];
                       mygrid1.Rows[i].Cells["VBP"].Value = dr["VBETPROPRETE"];
                       mygrid1.Rows[i].Cells["VB350"].Value = dr["VB350"];
                       mygrid1.Rows[i].Cells["VTOTAL"].Value = dr["VTOTAL"];
                       mygrid1.Rows[i].Cells["U"].Value = dr["U"];
                       mygrid1.Rows[i].Cells["PUHTF"].Value = dr["PUHT"];
                       mygrid1.Rows[i].Cells["PTHT"].Value = dr["PTHT"];
                       mygrid1.Rows[i].Cells["TVAFF"].Value = dr["TVA"];
                       mygrid1.Rows[i].Cells["ID"].Value = dr["ID"];
                       i++;
                   }
               }
               catch { }
               mygrid1.Focus();
           }
        }

        private void increment()
        {
            String req = "SELECT * FROM pnumste where codes='" + Program.Societe + "' and codee = '" + Program.Exercice + "'";
            DataSet ds = met.recuperer_table(req, "pnumste");
            int index = 0;
            if (ds != null)
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        index = ds.Tables[0].Rows.Count - 1;

                    }
                }
            string s1 = ds.Tables[0].Rows[index].Field<Object>("dfacture") + "";
            string s2 = ds.Tables[0].Rows[index].Field<Object>("facture") + "";
            int num = int.Parse(s2);
            num++;
            string s = num.ToString().Trim();
            int l1 = s2.Trim().Length;
            int l2 = s.Length;
            for (int i = l2; i < l1; i++)
                s = "0" + s;
            s = s1 + s;
            NumFact = s;
        }

        public  void SaveIncrementationNumero()
        {
            String req = "SELECT * FROM pnumste where codes='" + Program.Societe + "' and codee = '" + Program.Exercice + "'";
            DataSet dsi = met.recuperer_table(req, "pnumste");
            string s2 = dsi.Tables[0].Rows[0].Field<Object>("facture") + "";
            String snum = NumFact.Substring(2, NumFact.Length - 2);
            int anum = int.Parse(snum);
            int xnum = int.Parse(s2);
            xnum++;
            if (anum >= xnum)
            {
                string s = anum.ToString().Trim();
                int l1 = s2.Trim().Length;
                int l2 = s.Length;
                for (int ii = l2; ii < l1; ii++)
                    s = "0" + s;
                string sqlinc = "UPDATE pnumste SET facture = '" + s + "' where codes='" + Program.Societe + "' and codee = '" + Program.Exercice + "'";
                met.Execute(sqlinc);
            }
            else {
                increment();
                SaveIncrementationNumero();
                nfact.Text = NumFact;
            }
        }

        private void tref_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
            {
                tnref.Focus();
            }
        }

        private void tnref_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
            {
                tvref.Focus();
            }
        }

        private void tvref_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
            {
                Tcodc.Focus();
            }
        }

        private void tprorata_TextChanged(object sender, EventArgs e)
        {
            calcultotal();
        }

        private void Tpret_TextChanged(object sender, EventArgs e)
        {
            calcultotal();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            rpt.frm_fondation ff = new rpt.frm_fondation();
            ff.numf = nfact.Text + "";
            ff.ShowDialog();
        }

        private void tprorata_Validated(object sender, EventArgs e)
        {
            double t1 = 0;
            double.TryParse(tprorata.Text, out t1);
            tprorata.Text = t1.ToString("N2");
        }

        private void Tpret_Validated(object sender, EventArgs e)
        {
            double t1 = 0;
            double.TryParse(Tpret.Text, out t1);
            Tpret.Text = t1.ToString("N2");
        }

        private void tpav_Validated(object sender, EventArgs e)
        {
            double t1 = 0;
            double.TryParse(tpav.Text, out t1);
            tpav.Text = t1.ToString("N2");
        }

        private void rbnht_CheckedChanged(object sender, EventArgs e)
        {
            typecalc = "NHT";
            calcultotal();
        }

        private void rbht_CheckedChanged(object sender, EventArgs e)
        {
            typecalc = "HT";
            calcultotal();
        }

    }
}