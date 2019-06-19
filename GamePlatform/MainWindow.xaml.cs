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
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace GamePlatform
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GamesLoad();
        }

        public async void GamesLoad()
        {
            List<Item> gamesList = new List<Item>();
            Rss rssResult = await LoadInfo();  //await - признак асинхронной задачи
            gamesList = rssResult.Channel.Items;
            gamesDataGrid.ItemsSource = gamesList;
        }

        private Task<Rss> LoadInfo()
        {
            Rss rss = null;
            // см. http://qaru.site/questions/44548/user-xmlns-was-not-expected-deserializing-twitter-xml
            XmlRootAttribute xRoot = new XmlRootAttribute("rss");
            XmlSerializer formatter = new XmlSerializer(typeof(Rss), xRoot);

            WebRequest req = WebRequest.Create(@"https://gamer-info.com/rssflash/");
            WebResponse response = req.GetResponse();
            using (Stream s = response.GetResponseStream())  // пишем в поток.
            {
                using (StreamReader r = new StreamReader(s))  // читаем из потока.
                {
                    rss = (Rss)formatter.Deserialize(r);
                }
            }

            response.Close(); // закрываем поток

            return Task.FromResult(rss);  // правильно так писать
        }


        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GamesLoad();

        }
    }
}
