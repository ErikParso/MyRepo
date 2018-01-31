using BattleshipsServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BattleshipsClient.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private PlayerContract player;

        public LoginWindow()
        {
            InitializeComponent();
        }

        public static PlayerContract Run()
        {
            LoginWindow l = new LoginWindow();
            l.ShowDialog();
            return l.player;
        }

        public void LoginClick()
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                player = client.Login(TbName.Text, TbPassword.Text);
            }

            if (player == null)
                Info.Content = "Invalid name or password";
            else
                Close();
        }

        public void RegisterClick()
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                player = client.Register(TbName.Text, TbPassword.Text);
            }

            if (player == null)
                Info.Content = $"Player with name {TbName.Text} is already registered";
            else
                Close();
        }
    }
}
