using BudgetTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetTracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesList : ContentPage
    {
        public CategoriesList()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            var budget = (Budget)BindingContext;
            if (budget != null && !string.IsNullOrEmpty(budget.FileName))
            {
                String allText = File.ReadAllText(budget.FileName);
                String[] text = allText.Split('\n');
                //Name.Text  = File.ReadAllText(budget.FileName);
                //amount.Text=File.ReadAllText(budget.FileName);
                //date.Text=File.ReadAllText(budget.FileName);
                Name.Text = text[0];
                amount.Text = text[1];
                date.Text = text[2];
            }
        }
        
        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            var budget= (Budget)BindingContext;
            if (budget == null || string.IsNullOrEmpty(budget.FileName))
            {
                budget = new Budget();
                budget.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    $"{Path.GetRandomFileName()}.notes.txt");
            }
            File.WriteAllText(budget.FileName,Name.Text);
            // File.WriteAllText(budget.FileName, amount.Text);
            //File.WriteAllText(budget.FileName, date.Text);
           // File.AppendText(budget.FileName, amount.Text);
            File.AppendAllText(budget.FileName,"\n"+amount.Text);
            File.AppendAllText(budget.FileName,"\n"+date.Text);
            
            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                Shell.Current.CurrentItem = (Shell.Current as AppShell).MainPageContent;
            }
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            var budget = (Budget)BindingContext;
            if (File.Exists(budget.FileName))
            {
                File.Delete(budget.FileName);
            }
            Name.Text= string.Empty;
            amount.Text= string.Empty;
            date.Text= string.Empty;
            Navigation.PopModalAsync();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            date.Text = e.NewDate.ToString();

        }
    }
}