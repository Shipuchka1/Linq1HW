using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Linq1HW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string conStr = @"Data Source = DESKTOP-NESB2OL\SQLEXPRESS; Initial Catalog = Linq1; User Id = Natalya; Password = 12345";
        public MainWindow()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                con.Open();
                
                SqlDataAdapter da = new SqlDataAdapter("Select * from Area", con);
                List<Area> areas = new List<Area>();
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                PropertyInfo[] prop = typeof(Area).GetProperties();

                foreach (PropertyInfo item in prop)
                {
                    GridViewColumn temp = new GridViewColumn();
                    temp.Header = item.Name;
                    temp.DisplayMemberBinding = new Binding(item.Name);
                    DataGridView.Columns.Add(temp);
                }
                int? k = 0; string str = "";
                foreach (DataRow row in dt.Rows)
                {
                    int i = 0;
                    Area temp = new Area();
                    foreach (PropertyInfo item in prop)
                    {
                        
                        if (row.ItemArray[i].GetType() == typeof(System.DBNull))
                        {
                            item.SetValue(temp, null);
                        }
                        else if (row.ItemArray[i].GetType() == typeof(System.Boolean))
                        {
                            if ((Boolean)row.ItemArray[i])
                            {
                                item.SetValue(temp, 1);


                            }
                            else
                            {
                                item.SetValue(temp, 0);
                            }
                            
                        }
                        else
                        {
                            item.SetValue(temp, row.ItemArray[i]);
                        }
                        i++;
                    }
                  
                    areas.Add(temp);
                }

                DataListView.ItemsSource = areas;
                //Task4(areas, DataGridView, DataListView);
                //Task5(areas, DataGridView, DataListView);
                //Task6(areas, DataGridView, DataListView);
                //Task7(areas, DataGridView, DataListView);
                //Task8(areas, DataGridView, DataListView);
                Task9(areas, DataGridView, DataListView);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static void Task4(List<Area> areas, GridView GW, ListView LW)
        {
            GW.Columns.Clear();
            GW.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name")});
            GW.Columns.Add(new GridViewColumn() { Header = "FullName", DisplayMemberBinding = new Binding("FullName") });
            GW.Columns.Add(new GridViewColumn() { Header = "IP", DisplayMemberBinding = new Binding("IP") });
            var res = areas.Where(w => w.TypeArea == 1).OrderByDescending(o => o.AreaId).Select(s => new
            {
                s.Name,
                s.FullName,
                s.IP
            });

            LW.ItemsSource = res.ToList();
        }

        public static void Task5(List<Area> areas, GridView GW, ListView LW)
        {
            GW.Columns.Clear();
            GW.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name") });
            GW.Columns.Add(new GridViewColumn() { Header = "FullName", DisplayMemberBinding = new Binding("FullName") });
            GW.Columns.Add(new GridViewColumn() { Header = "IP", DisplayMemberBinding = new Binding("IP") });

            var res = (from d in areas
                       where d.ParentId == 0
                       select new {
                           d.Name,
                           d.FullName,
                           d.IP
                       });

            LW.ItemsSource = res.ToList();
        }

        public static void Task6(List<Area> areas, GridView GW, ListView LW)
        {
            int[] pavilion = new int[] { 1, 2, 3, 4, 5, 6 };

            GW.Columns.Clear();
            GW.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name") });
            GW.Columns.Add(new GridViewColumn() { Header = "PavilionId", DisplayMemberBinding = new Binding("PavilionId") });
            GW.Columns.Add(new GridViewColumn() { Header = "FullName", DisplayMemberBinding = new Binding("FullName") });
            GW.Columns.Add(new GridViewColumn() { Header = "IP", DisplayMemberBinding = new Binding("IP") });

            var res = areas.Where(w => (pavilion.Where(w2=>w2==2||w2==4||w2==6)).Contains(w.PavilionId));

            LW.ItemsSource = res.ToList();
        }

        public static void Task7(List<Area> areas, GridView GW, ListView LW)
        {
            int[] pavilion = new int[] { 1, 2, 3, 4, 5, 6 };

            GW.Columns.Clear();
            GW.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name") });
            GW.Columns.Add(new GridViewColumn() { Header = "PavilionId", DisplayMemberBinding = new Binding("PavilionId") });
            GW.Columns.Add(new GridViewColumn() { Header = "FullName", DisplayMemberBinding = new Binding("FullName") });
            GW.Columns.Add(new GridViewColumn() { Header = "IP", DisplayMemberBinding = new Binding("IP") });

            // var res = areas.Where(w => (pavilion.Where(w2 => w2 == 2 || w2 == 4 || w2 == 6)).Contains(w.PavilionId));
            var res = from n in areas
                      where
                      (from p in pavilion
                       where p % 2 == 0
                       select p).Contains(n.PavilionId)
                      select n;
            LW.ItemsSource = res.ToList();
        }

        public static void Task8(List<Area> areas, GridView GW, ListView LW)
        {
            GW.Columns.Clear();
            GW.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name") });
            GW.Columns.Add(new GridViewColumn() { Header = "WorkingPeople", DisplayMemberBinding = new Binding("WorkingPeople") });
            GW.Columns.Add(new GridViewColumn() { Header = "FullName", DisplayMemberBinding = new Binding("FullName") });
            GW.Columns.Add(new GridViewColumn() { Header = "IP", DisplayMemberBinding = new Binding("IP") });

            var res = from a in areas
                      let i = 1
                      where a.WorkingPeople > i
                      select new
                      {
                          a.Name,
                          a.WorkingPeople,
                          a.FullName,
                          a.IP
                      };

            LW.ItemsSource = res.ToList();
        }

        public static void Task9(List<Area> areas, GridView GW, ListView LW)
        {
            GW.Columns.Clear();
            GW.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name") });
            GW.Columns.Add(new GridViewColumn() { Header = "Dependence", DisplayMemberBinding = new Binding("Dependence") });
            GW.Columns.Add(new GridViewColumn() { Header = "FullName", DisplayMemberBinding = new Binding("FullName") });
            GW.Columns.Add(new GridViewColumn() { Header = "ParentId", DisplayMemberBinding = new Binding("ParentId") });

            var res = from a in areas
                      select new
                      {
                          a.Name,
                          a.Dependence,
                          a.FullName,
                          a.ParentId
                      }
                      into r
                      where r.Dependence > 0
                      select new
                      {
                          r.Name,
                          r.Dependence,
                          r.FullName,
                          r.ParentId
                      };
            LW.ItemsSource = res.ToList();
        }

        public class Area
        {
            public int AreaId { get; set; }
            public int? TypeArea { get; set; }
            public string Name { get; set; }
            public int? ParentId { get; set; }
            public int? NoSplit { get; set; }
            public int? AssemblyArea { get; set; }
            public string FullName { get; set; }
            public int? MultipleOrders { get; set; }
            public int? HiddenArea { get; set; }
            public string IP { get; set; }
            public int PavilionId { get; set; }
            public int TypeId { get; set; }
            public int? OrderExecution { get; set; }
            public int? Dependence { get; set; }
            public int? WorkingPeople { get; set; }
            public int? ComponentTypeId { get; set; }
            public int? GroupId { get; set; }
            public int? Segment { get; set; }
        }
    }
}
