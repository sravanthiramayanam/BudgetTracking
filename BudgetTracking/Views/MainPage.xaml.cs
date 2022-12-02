using BudgetTracking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetTracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public string FileName { get; private set; }
        public double totalbudget;
        public dynamic SelectedMonth = DateTime.Now.Month - 1;

        public MainPage()
        {
            InitializeComponent();
            //double totalexpenses = 0;
            //double remainingbudget = totalbudget - totalexpenses;
           
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                $"{SelectedMonth}.budget.txt");
            
            if (File.Exists(fileName))
            {
               MonthlyBudget.IsVisible = true;
               SaveMonthlyBudget.IsVisible = true;
              // BudgetLabel.IsVisible = false;
                totalbudget = double.Parse(File.ReadAllText(fileName));
                //yourbudget.Text = File.ReadAllText(fileName);
                MonthlyBudget.Text = File.ReadAllText(fileName);
                MonthPicker.SelectedIndex = DateTime.Now.Month - 1;



            }
            else
            {
                
                //MonthlyBudget.IsVisible = true;
                //SaveMonthlyBudget.IsVisible = true;
                //BudgetLabel.IsVisible = false;
                //yourbudget.IsVisible = false;

                MonthPicker.SelectedIndex = DateTime.Now.Month - 1;

            }

            
        }


        protected override void OnAppearing()
        {
            LoadUI();
            
        }

        public void LoadUI()
        {
            SelectedMonth = MonthPicker.SelectedIndex;

            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                $"{SelectedMonth}.budget.txt");

            if (File.Exists(fileName))
            {
                MonthlyBudget.IsVisible = true;
                SaveMonthlyBudget.IsVisible = true;
                // BudgetLabel.IsVisible = false;
                totalbudget = double.Parse(File.ReadAllText(fileName).Trim());
                //yourbudget.Text = File.ReadAllText(fileName);
                MonthlyBudget.Text = File.ReadAllText(fileName);
                //MonthPicker.SelectedIndex = DateTime.Now.Month - 1;


                var budgets = new List<Budget>();
                var files = Directory.EnumerateFiles(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), $"*.{SelectedMonth}.notes.txt");
                double totalexpenses = 0;

                foreach (var file in files)
                {
                    String allText = File.ReadAllText(file);
                    String[] text = allText.Split('\n');

                    var Name = text[0];
                    var amount = text[1];
                    DateTime dateselected= DateTime.Parse(text[2]);
                    var image = "";
                    try
                    {
                        image = text[4];
                    }
                    catch
                    {
                        image = "";
                    }


                    if (amount == "")
                    {
                        totalexpenses += 0.0;
                    }
                    else
                    {
                        totalexpenses += double.Parse(amount);
                    }

                    var budget = new Budget
                    {

                        Text = Name,                     
                        amount = "$" + amount,
                        ImageUrl = image,
                        Date=dateselected,
                        FileName = file
                    };

                    budgets.Add(budget);

                }
                double remainingbudget = totalbudget - totalexpenses;
                Leftbudget.Text = "$" + remainingbudget.ToString();

                if (totalexpenses > totalbudget)
                {
                    DisplayAlert("Warning", "OVER THE BUDGET", "ok");
                }
                BudgetListView.ItemsSource = budgets.OrderByDescending(t => t.Date);




            }
            else
            {

                MonthlyBudget.IsVisible = true;
                SaveMonthlyBudget.IsVisible = true;
                MonthlyBudget.Text = string.Empty;
                Leftbudget.Text = "Setup monthly budget";
                var budgets = new List<Budget>();
                var files = Directory.EnumerateFiles(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), $"*.{SelectedMonth}.notes.txt");
                double totalexpenses = 0;

                foreach (var file in files)
                {
                    String allText = File.ReadAllText(file);
                    String[] text = allText.Split('\n');

                    var Name = text[0];
                    var amount = text[1];
                    DateTime dateselected = DateTime.Parse(text[2]);
                    var image = "";
                    try
                    {
                        image = text[4];
                    }
                    catch
                    {
                        image = "";
                    }


                    if (amount == "")
                    {
                        totalexpenses += 0.0;
                    }
                    else
                    {
                        totalexpenses += double.Parse(amount);
                    }

                    var budget = new Budget
                    {

                        Text = Name,
                        amount = "$" + amount,
                        ImageUrl = image,
                        Date = dateselected,
                        FileName = file
                    };

                    budgets.Add(budget);

                }
                
                BudgetListView.ItemsSource = budgets.OrderByDescending(t => t.Date);


            }


        }


        private async void BudgetListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushModalAsync(new CategoriesList
            {
                BindingContext = (Budget)e.SelectedItem
            });
        }

        private async void SaveMonthlyBudgetClicked(object sender, EventArgs e)
        {
            SelectedMonth = MonthPicker.SelectedIndex;
            var BudgetForMonth = MonthlyBudget.Text;
            
                var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                 $"{SelectedMonth}.budget.txt");
               
                try
                {
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine(BudgetForMonth);
                    sw.Close();
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    System.IO.Directory.CreateDirectory(BudgetForMonth);
                    StreamWriter sw = new System.IO.StreamWriter(fileName);
                    sw.WriteLine(BudgetForMonth);
                    sw.Close();
                }

            //MonthlyBudget.IsVisible = false;
            //SaveMonthlyBudget.IsVisible = false;
            //yourbudget.Text = BudgetForMonth;
            MonthlyBudget.IsVisible = true;
            SaveMonthlyBudget.IsVisible = true;
            
            if (Navigation.ModalStack.Count > 0)
                {
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Shell.Current.CurrentItem = (Shell.Current as AppShell).MainPageContent;
                }

            LoadUI();

        }

        private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUI();
        }
    }
}