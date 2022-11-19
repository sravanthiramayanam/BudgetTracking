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

        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            var budgets = new List<Budget>();
            var files = Directory.EnumerateFiles(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "*.notes.txt");

            foreach (var file in files)
            {
               // String allText = File.ReadAllText(file);
               // String[] text = allText.Split('\n');
                var budget= new Budget
                {
                   // Text= text[0],
                    //amount="$"+text[1],
                    //Date = text[2],

                    Text = File.ReadAllText(file),
                    //Date = File.GetCreationTime(file),
                  //amount = File.ReadAllText(file),


                  //Name.Text = text[0];
                  //amount.Text = text[1];
                  // date.Text = text[2];

                FileName = file
                };
              budgets.Add(budget);  
                
            }
            BudgetListView.ItemsSource = budgets.OrderByDescending(t => t.Date);
            
            //TodoListView.ItemsSource = todos.OrderByDescending(t => t.Date);
        }
        private void BudgetListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushModalAsync(new CategoriesList
            {
                BindingContext = (Budget)e.SelectedItem
            }).Wait();
        }

        private async void SaveMonthlyBudgetClicked(object sender, EventArgs e)
        {
            var budget = (Budget)BindingContext;
            budget = new Budget();
            
                budget.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                $"{Path.GetRandomFileName()}.budget.txt");
            
                File.WriteAllText(budget.FileName, MonthlyBudget.Text);
                if (Navigation.ModalStack.Count > 0)
                {
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Shell.Current.CurrentItem = (Shell.Current as AppShell).MainPageContent;
                }


            
            

        }
    }
}