using BudgetTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BudgetTracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesList : ContentPage
    {
        public List<Category> ListOfCategory { get; set; }
        public Category currentCategory;
        public CategoriesList()
        {
            InitializeComponent();
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
               "budget.txt");
            yourbudget.Text = File.ReadAllText(fileName);

            ListOfCategory = new List<Category>();
            ListOfCategory.Add(new Category
            {
                Name = "Groceries",
                ImageUrl = "groceries.png"

            });
            ListOfCategory.Add(new Category
            {
                Name = "Medical",
                ImageUrl = "medical.png"

            });
            ListOfCategory.Add(new Category
            {
                Name = "Automobile",
                ImageUrl = "automobile.png"

            });
            ListOfCategory.Add(new Category
            {
                Name = "Utility",
                ImageUrl = "utility.png"

            });
            
            CategoryListView.ItemsSource=ListOfCategory;


        }
        protected override void OnAppearing()
        {
            var budget = (Budget)BindingContext;
            if (budget != null && !string.IsNullOrEmpty(budget.FileName))
            {
                String allText = File.ReadAllText(budget.FileName);
                String[] text = allText.Split('\n');
                Name.Text = text[0];
                amount.Text = text[1];
                date.Text = text[2];
                selectedcategory.Text = text[3];           

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
            if(amount.Text=="")
            {
                File.AppendAllText(budget.FileName, "\n" + 0.0);
            }
            else
            {
                File.AppendAllText(budget.FileName, "\n" + amount.Text);
            }
            
            File.AppendAllText(budget.FileName,"\n"+date.Text);
            File.AppendAllText(budget.FileName,"\n"+selectedcategory.Text);
            File.AppendAllText(budget.FileName, "\n" + currentCategory.ImageUrl);
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

        private void CategoryListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            currentCategory = e.SelectedItem as Category;
            selectedcategory.Text= currentCategory.Name;    
        }
    }
}