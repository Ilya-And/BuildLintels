using System;
using System.Collections.Generic;
using System.Linq;
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
using Autodesk.Revit.DB;

namespace BuildLintels
{
    /// <summary>
    /// Логика взаимодействия для UserWindDoor.xaml
    /// </summary>
    public partial class UserWind : Window
    {
        Dictionary<string, Dictionary<string, List<FamilyInstance>>> modelGroups;

        public UserWind (Dictionary<string, Dictionary<string, List<FamilyInstance>>> elements)
        {
            InitializeComponent();

            modelGroups = elements;

            foreach (string groupWallsName in modelGroups.Keys)
            {
                var listOfCategories = new List<string>();
                var box = new ListBox();



                foreach (string k in modelGroups[groupWallsName].Keys)
                {
                    if (!listOfCategories.Contains(k))
                    {
                        listOfCategories.Add(k);

                    }


                }





                box.ItemsSource = listOfCategories;

                lintels.Items.Add(new TabItem
                {
                    Header = new TextBlock { Text = groupWallsName },

                    Content = box
                });
            }
            //AllDoorsView.ItemsSource = modelGroups;
        }

    }
}
