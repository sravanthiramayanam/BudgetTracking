
using BudgetTracking.Models;
using BudgetTracking.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BudgetTracking
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ShellContent MainPageContent;
        public ShellContent ExpensePageContent;
        public AppShell()
        {
            InitializeComponent();
            MainPageContent = Home;
            ExpensePageContent = CategoriesList;
        }

    }
}
