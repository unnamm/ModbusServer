using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI.View;

namespace UI
{
    internal class App : Application
    {
        private readonly IServiceCollection _services;
        private readonly Dictionary<Type, Type> _viewPair = [];

        public App()
        {
            var builder = Host.CreateApplicationBuilder();
            _services = builder.Services;

            //add view
            _services.AddSingleton<MainWindow>();

            //add view and viewmodel

            //add other

            var host = builder.Build();
            Ioc.Default.ConfigureServices(host.Services);

            AutoConnectViewAndViewModel();
            SetDefaultResource();

            Startup += (x, y) => Ioc.Default.GetService<MainWindow>()!.Show(); //mainwindow show
        }

        /// <summary>
        /// auto connect view and viewmodel
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void AutoConnectViewAndViewModel()
        {
            foreach (var pair in _viewPair)
            {
                var uc = (ContentControl)Ioc.Default.GetService(pair.Key)!;
                if (uc.DataContext != null)
                {
                    throw new Exception($"{uc} is already allocated DataContext");
                }
                uc.DataContext = Ioc.Default.GetService(pair.Value) ?? throw new Exception("viewmodel null");
            }
        }

        /// <summary>
        /// add view and viewmodel
        /// </summary>
        /// <typeparam name="View"></typeparam>
        /// <typeparam name="ViewModel"></typeparam>
        private void AddViewAndViewModel<View, ViewModel>() where View : ContentControl where ViewModel : class
        {
            _services.AddSingleton<View>();
            _services.AddSingleton<ViewModel>();

            _viewPair.Add(typeof(View), typeof(ViewModel));
        }

        private void SetDefaultResource()
        {
            ResourceDictionary resourceDictionary = [];
            ResourceDictionary materialDesignDefaults = new()
            {
                Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml")
            };
            resourceDictionary.MergedDictionaries.Add(materialDesignDefaults);

            base.Resources = resourceDictionary;
        }
    }
}
