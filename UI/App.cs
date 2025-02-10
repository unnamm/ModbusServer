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
        private readonly MainWindow _mainView;
        private readonly List<Type> _viewList = [];
        private readonly IServiceCollection _services;
        private readonly Dictionary<Type, Type> _viewPair = [];

        public App()
        {
            var builder = Host.CreateApplicationBuilder();
            _services = builder.Services;

            #region host build
            //other

            //add tab view
            AddView<MainWindow>();

            //add tab view and viewmodel

            #endregion

            var host = builder.Build();
            Ioc.Default.ConfigureServices(host.Services); //setting default
            _mainView = Ioc.Default.GetService<MainWindow>()!; //set mainview

            SettingView();

            Startup += (x, y) => _mainView.Show();
        }

        /// <summary>
        /// auto connect view and viewmodel
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void SettingView()
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

        /// <summary>
        /// add only view
        /// </summary>
        /// <typeparam name="View"></typeparam>
        private void AddView<View>() where View : ContentControl
        {
            _services.AddSingleton<View>();
            _viewList.Add(typeof(View));
        }
    }
}
