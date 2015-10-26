using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace GetTaiwanStreetName
{
    public partial class Form1 : Form
    {
        private DataTable cityDT;
        private DataTable cityareaDT;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tab 1
            cityDT = Utilities.GetCity();
            cityareaDT = Utilities.GetCityarea();
            dataGridView1.DataSource = cityDT;
            dataGridView1.Columns["Id"].Visible = false;

            //tab 2
            comboBox1.DataSource = cityDT.AsEnumerable()
                .Select(x => new
                {
                    Text = x.Field<string>("City"),
                    Value = x.Field<int>("Id")
                }).ToList();

            //tab 3
            comboBox2.DataSource = cityDT.AsEnumerable()
                .Select(x => new
                {
                    Text = x.Field<string>("City"),
                    Value = x.Field<int>("Id")
                }).ToList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            dataGridView2.DataSource = cityareaDT.Select("CityId=" + cb.SelectedValue.ToString()).CopyToDataTable();
            dataGridView2.Columns["CityId"].Visible = false;
            //cityareaDT.AsEnumerable().Select(x => x.Field<string>("Cityarea")).ToList();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            DataTable filterArea = cityareaDT.Select("CityId=" + cb.SelectedValue.ToString()).CopyToDataTable();
            comboBox3.DataSource = filterArea.AsEnumerable()
                .Select(x => new
                {
                    Text = x.Field<string>("Cityarea"),
                    Value = x.Field<int>("CityId")
                }).ToList();
        }

        private async void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string city = comboBox2.Text;
            string cityarea = comboBox3.Text;
            string result = await Utilities.GetAddress(city, cityarea);

            var source = new BindingSource();
            List<string> address = Utilities.ParserAddress(result);
            source.DataSource = address.Select(x => new { Address = x });
            dataGridView3.DataSource = source;
        }

        //private async void button1_Click(object sender, EventArgs e)
        //{
        //    Stream myStream;
        //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //    saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
        //    //saveFileDialog1.FilterIndex = 2;
        //    saveFileDialog1.RestoreDirectory = true;
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        if ((myStream = saveFileDialog1.OpenFile()) != null)
        //        {
        //            button1.Enabled = false;
        //            button1.Text += "(資料擷取中…)";

        //            ConcurrentBag<OutputAddress> bag = new ConcurrentBag<OutputAddress>();
        //            #region GetRemoteData
        //            foreach (DataRow cityRow in cityDT.Rows)
        //            {
        //                string CityId = cityRow.Field<int>("Id").ToString();
        //                DataRow[] cityareaRows = cityareaDT.Select("CityId=" + CityId);
        //                int i = 1;
        //                foreach (var cityareaRow in cityareaRows)
        //                {
        //                    string city = cityRow.Field<string>("City");
        //                    string cityarea = cityareaRow.Field<string>("Cityarea");
        //                    string result = await Utilities.GetAddress(city, cityarea);
        //                    List<string> address = Utilities.ParserAddress(result);


        //                    foreach (var addr in address)
        //                    {
        //                        OutputAddress oa = new OutputAddress
        //                        {
        //                            City = string.Format("{0:000}_{1}", Convert.ToInt16(CityId), city),
        //                            CityArea = cityarea, //string.Format("{0:000}_{1}", i, cityarea),
        //                            Address = addr
        //                        };
        //                        bag.Add(oa);
        //                        i++;
        //                    }
        //                }
        //            }
        //            #endregion

        //            StringBuilder sb = new StringBuilder();
        //            foreach (var item in bag.Reverse())
        //            {
        //                sb.AppendFormat("{0},{1},{2}\r\n", item.City, item.CityArea, item.Address);
        //            }
        //            string output = sb.ToString();

        //            await myStream.WriteAsync(Encoding.Unicode.GetBytes(output), 0, Encoding.Unicode.GetByteCount(output));
        //            myStream.Close();

        //            button1.Enabled = true;
        //            button1.Text = "輸出資料";
        //        }
        //    }
        //}

        private async void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var path = fbd.SelectedPath;

                button1.Enabled = false;
                button1.Text += "(資料擷取中…)";

                List<CityModel> bag = new List<CityModel>();
                #region GetRemoteData
                foreach (DataRow cityRow in cityDT.Rows)
                {
                    CityModel city = new CityModel() { CityName = cityRow.Field<string>("City") };
                    string CityId = cityRow.Field<int>("Id").ToString();
                    DataRow[] cityareaRows = cityareaDT.Select("CityId=" + CityId);
                    foreach (var cityareaRow in cityareaRows)
                    {
                        CityAreaModel area = new CityAreaModel { CityAreaName = cityareaRow.Field<string>("Cityarea") };
                        string result = await Utilities.GetAddress(city.CityName, area.CityAreaName);
                        List<string> address = Utilities.ParserAddress(result);
                        foreach (var addr in address)
                        {
                            StreetModel street = new StreetModel { StreetName = addr };
                            area.Street.Add(street);
                        }
                        city.CityArea.Add(area);
                    }
                    bag.Add(city);
                }
                #endregion
                string output = JsonConvert.SerializeObject(bag);

                System.IO.File.WriteAllText(System.IO.Path.Combine(path, "taiwan.json"), output, Encoding.UTF8);

                button1.Enabled = true;
                button1.Text = "輸出資料";
            }
        }
    }
}
